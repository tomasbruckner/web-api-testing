using System;
using System.IdentityModel.Tokens.Jwt;
using Example.Api.Tests.Seeders;
using Example.Api.Tests.Support.MockServices;
using Example.Data.Data;
using Example.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Example.Api.Tests.Support.Utils
{
    public class CustomWebApplicationFactoryWithInMemoryDb<TStartup> : WebApplicationFactory<Startup>
        where TStartup : class
    {
        public const string TestDbName = "InMemoryDbForTesting";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(
                services =>
                {
                    // constant time in all integration testing
                    services.Replace(
                        new ServiceDescriptor(
                            typeof(ITimeService),
                            typeof(TimeServiceMock),
                            ServiceLifetime.Transient
                        )
                    );
                }
            );

            builder.ConfigureServices(
                services =>
                {
                    var serviceProvider = new ServiceCollection()
                        .AddEntityFrameworkInMemoryDatabase()
                        .BuildServiceProvider();

                    services.AddDbContext<RepositoryContext>(
                        options =>
                        {
                            options.UseInMemoryDatabase(TestDbName)
                                .ConfigureWarnings(
                                    x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)
                                );
                            options.UseInternalServiceProvider(serviceProvider);
                        }
                    );

                    services.PostConfigure<JwtBearerOptions>(
                        JwtBearerDefaults.AuthenticationScheme,
                        options =>
                        {
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                                SignatureValidator = (token, parameters) => new JwtSecurityToken(token),
                                ValidateIssuer = false,
                                ValidateLifetime = false,
                                ValidateIssuerSigningKey = false,
                                ValidateAudience = false
                            };
                        }
                    );

                    using var scope = services.BuildServiceProvider().CreateScope();
                    var appDb = scope.ServiceProvider.GetRequiredService<RepositoryContext>();
                    appDb.Database.EnsureDeleted();
                    appDb.Database.EnsureCreated();

                    try
                    {
                        CommonSeeder.Seed(appDb);
                    }
                    catch (Exception ex)
                    {
                        var logger = scope.ServiceProvider
                            .GetRequiredService<ILogger<CustomWebApplicationFactoryWithInMemoryDb<TStartup>>>();
                        logger.LogError(
                            ex,
                            "An error occurred while seeding the database with messages. Error: {ex.Message}"
                        );
                    }
                }
            );
        }
    }
}
