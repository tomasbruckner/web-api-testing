using System;
using System.Text;
using Example.Common.Enums;
using Example.Common.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Example.Api.Security
{
    public static class SecurityExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var encodedKey =
                Encoding.ASCII.GetBytes(
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(configuration.GetValue<string>("Jwt:Secret"))
                    )
                );

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                    options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = configuration.GetValue<string>("Jwt:Issuer"),
                            ValidAudience = configuration.GetValue<string>("Jwt:Issuer"),
                            IssuerSigningKey = new SymmetricSecurityKey(encodedKey)
                        };
                    }
                );

            services.AddAuthorization(
                authorizationOptions =>
                {
                    authorizationOptions.AddPolicy(
                        PolicyConstants.OnlyAdmin,
                        policy => policy.RequireRole(RoleEnum.Admin.GetDescription())
                    );

                    authorizationOptions.AddPolicy(
                        PolicyConstants.AnyRole,
                        policy => policy.RequireRole(
                            RoleEnum.Admin.GetDescription(),
                            RoleEnum.User.GetDescription()
                        )
                    );
                }
            );
        }
    }
}
