using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
namespace Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>();
            return services;
        }
    }
}