using Example.Services.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Services.Services.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<ITimeService, TimeService>();
            services.AddTransient<IUserService, UserService>();
        }
    }
}
