using System;
using Example.Data.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Api.Tests.Support.Utils
{
    public static class TestExtensions
    {
        public static void SeedData(
            this CustomWebApplicationFactoryWithInMemoryDb<Startup> factory,
            Action<RepositoryContext> func
        )
        {
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<RepositoryContext>();
                func(db);

                db.SaveChanges();
            }
        }
    }
}
