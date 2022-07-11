﻿using Invoice.Api.AutoMapper;
using Invoice.Api.Configuration;
using Invoice.Application.Interfaces;
using Invoice.Application.Services;
using Invoice.Domain.Interfaces;
using Invoice.Domain.Services;
using Invoice.Infra.Configuration;
using Invoice.Infra.Data;
using Invoice.Infra.Interfaces;
using Invoice.Infra.Repositories;
using Invoice.Infra.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Invoice.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        public static IServiceCollection AddCache(this IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddSingleton<ICacheService, MemoryCacheService>();

            return services;
        }

        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            return services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddDbContext<EFContext>(options => options.UseMySql(configuration.GetConnectionString("DefaultConnection")));
        }

        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IValidatorService, ValidatorService>();
            services.AddScoped<IPasswordEncryption, PasswordEncryptService>();
            services.AddScoped<IPasswordService, UserPasswordService>();
            services.AddScoped<ITokenService, JWTokenService>();

            return services;
        }

        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CacheConfig>(configuration.GetSection("CacheConfig"));
            services.Configure<JWTConfig>(configuration.GetSection("JWTConfig"));

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("JWTConfig").Get<JWTConfig>();

            var key = Encoding.ASCII.GetBytes(jwtConfig.SecretKey);

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            services.AddAuthentication(c =>
            {
                c.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                c.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(c =>
             {
                 c.RequireHttpsMetadata = false;
                 c.SaveToken = true;
                 c.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(key),
                     ValidateIssuer = false,
                     ValidateAudience = false
                 };
             });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Invoice.Api", Description = "Invoice Core Api", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Please insert JWT with Bearer into field."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
            });

            return services;
        }

        public static IServiceCollection AddAutoMapperSetup(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DtoToResponseMappingProfile), typeof(EntityDtoMappingProfile));
            AutoMapperConfig.RegisterMappings();

            return services;
        }

    }
}