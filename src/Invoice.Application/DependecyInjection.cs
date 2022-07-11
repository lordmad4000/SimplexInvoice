using Invoice.Application.Interfaces;
using Invoice.Application.Services;
using Invoice.Domain.Interfaces;
using Invoice.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Invoice.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IValidatorService, ValidatorService>();
            services.AddScoped<IPasswordEncryption, PasswordEncryptService>();
            services.AddScoped<IPasswordService, UserPasswordService>();
            services.AddScoped<ITokenService, JWTokenService>();

            return services;            
        }        
    }
}