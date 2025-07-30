using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection.Emit;

namespace Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = typeof(ConfigureServices);
            string? cs = configuration.GetConnectionString("SqlConnectionString");
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(cs);
                // Tắt cảnh báo MultipleCollectionIncludeWarning
                opt.ConfigureWarnings(warnings =>
                    warnings.Ignore(RelationalEventId.MultipleCollectionIncludeWarning));
            });
            return services;
        }
    }
}