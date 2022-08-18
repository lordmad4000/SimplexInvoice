﻿using Invoice.Application.Common.Dto;
using System.Threading.Tasks;

namespace Invoice.Application.Interfaces
{
    public interface ILoginService
    {
        Task<UserDto> Login(string email, string password);
    }
}