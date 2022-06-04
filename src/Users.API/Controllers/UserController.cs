﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Users.API.Configuration;
using Users.API.Models.Request;
using Users.API.Models.Response;
using Users.Application.Interfaces;
using Users.Application.Models;
using Users.Domain.Exceptions;
using Users.Infra.Exceptions;

namespace Users.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly JWTConfig _jwtConfig;
        private readonly IMapper _mapper;
        public UserController(IUserService userService,
                              ITokenService tokenService,
                              IOptions<JWTConfig> jwtConfig,
                              IMapper mapper)
        {
            _userService = userService;
            _tokenService = tokenService;
            _jwtConfig = jwtConfig.Value;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            try
            {
                var userDto = await _userService.GetUserById(id);

                return (Ok(_mapper.Map<UserResponse>(userDto)));
            }
            catch (DataBaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userService.GetUsers();

                return (Ok(_mapper.Map<List<UserResponse>>(users)));
            }
            catch (DataBaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutUser([FromBody] UserUpdateRequest userUpdateRequest)
        {
            try
            {
                var userDto = _mapper.Map<UserDto>(userUpdateRequest);
                await _userService.UpdateUser(userDto);

                return (Ok(_mapper.Map<UserResponse>(userDto)));
            }
            catch (EntityValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DataBaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }

        [HttpPatch("PathReplaceUser/{id}")]
        public async Task<IActionResult> PatchReplaceUser([FromBody] JsonPatchDocument<UserDto> patchDoc, Guid id)
        {
            try
            {
                if (patchDoc == null)
                    return BadRequest("No field to update provided.");

                // ONLY ALLOWED REPLACE OPERATIONS
                patchDoc.Operations.RemoveAll(c => c.op != "replace");

                if (patchDoc.Operations.Count == 0)
                    return BadRequest("No field to update provided.");

                var userDto = await _userService.GetUserById(id, false);

                if (userDto == null)
                    return NotFound();

                patchDoc.ApplyTo(userDto, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                userDto = await _userService.PatchUser(userDto);

                return Ok(_mapper.Map<UserResponse>(userDto));
            }
            catch (EntityValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DataBaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var result = await _userService.DeleteUser(id);
                return (Ok(result));
            }
            catch (DataBaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] UserRegisterRequest userRegisterRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userDto = _mapper.Map<UserDto>(userRegisterRequest);
                userDto = await _userService.RegisterUser(userDto);
                var url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/{userDto.Id}";

                return (Created(url, _mapper.Map<UserResponse>(userDto)));
            }
            catch (EntityValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DataBaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("ActivateUser")]
        public async Task<IActionResult> ActivateUser()
        {
            try
            {
                string activationCode = "";
                var result = await _userService.ActivateUser(activationCode);

                return (Ok(result));
            }
            catch (DataBaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("Login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                var userDto = await _userService.Login(username, password);
                if (userDto == null)
                    return BadRequest("Invalid Username or Password.");

                var userLoginResponse = new UserLoginResponse
                {
                    Id = userDto.Id,
                    Token = _tokenService.GenerateToken(userDto.Password, userDto.Email, _jwtConfig.SecretKey)
                };

                return (await Task.FromResult(Ok(userLoginResponse)));
            }
            catch (DataBaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }


        //[AllowAnonymous]
        //[HttpPost("Login")]
        //public async Task<IActionResult> Login([FromBody] UserLoginRequest userLoginRequest)
        //{
        //    try
        //    {
        //        var userDto = await _userService.Login(userLoginRequest.Username, userLoginRequest.Password);
        //        if (userDto == null)
        //            return BadRequest("Invalid Username or Password.");

        //        var userLoginResponse = new UserLoginResponse
        //        {
        //            Id = userDto.Id,
        //            Token = _tokenService.GenerateToken(userDto.Password, userDto.Email, _jwtConfig.SecretKey)
        //        };

        //        return (await Task.FromResult(Ok(userLoginResponse)));
        //    }
        //    catch (DataBaseException ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.InnerException.Message);
        //    }
        //}

    }
}
