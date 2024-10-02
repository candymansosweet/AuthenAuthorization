namespace Web.App.API
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddWebAPI(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(config =>
            {
                config.CustomSchemaIds(type => type.FullName);
            });

            services.AddRouting(options => options.LowercaseUrls = true);
            return services;
        }
    }
}
