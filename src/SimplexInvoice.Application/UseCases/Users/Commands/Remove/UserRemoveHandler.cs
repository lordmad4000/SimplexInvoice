using AutoMapper;
using SimplexInvoice.Application.Common.Dto;
using SimplexInvoice.Application.Common.Interfaces.Persistance;
using SimplexInvoice.Application.Interfaces;
using SimplexInvoice.Domain.Exceptions;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System;
using SimplexInvoice.Domain.Users;
using SimplexInvoice.Domain.Users.Validations;

namespace SimplexInvoice.Application.Users.Commands
{

    public class UserRemoveHandler : IRequestHandler<UserRemoveCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidatorService _validatorService;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;
        private readonly ICustomLogger _logger;

        public UserRemoveHandler(IUserRepository userRepository,                        
                                 IUnitOfWork unitOfWork,
                                 IValidatorService validatorService,
                                 IPasswordService passwordService,
                                 IMapper mapper,
                                 ICustomLogger logger)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _validatorService = validatorService;
            _passwordService = passwordService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserDto> Handle(UserRemoveCommand request, CancellationToken cancellationToken)
        {
            var user = await Validate(request);
            if (!await _userRepository.DeleteAsync(user.Id))
                throw new Exception("User could not be removed succesfully.");

            await _unitOfWork.SaveChangesAsync();
            _logger.Debug(@$"User Removed with data: {user.ToString()}");

            return _mapper.Map<UserDto>(user);
        }

        private async Task<User> Validate(UserRemoveCommand request)
        {
            var user = await _userRepository.GetAsync(c => c.Id == request.Id, false);
            if (user is null)
                throw new NotFoundException("User not found.");

            _validatorService.ValidateModel(new UpdateUserValidator().Validate(user));

            return user;
        }

    }
}