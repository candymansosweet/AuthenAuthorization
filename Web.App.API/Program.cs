
using Application;
using Common.Models;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using Web.API.Middlewares;
using Web.App.API.Middlewares;

namespace Web.App.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

            // Add services to the container.
            builder.Services
                .AddInfrastructure(builder.Configuration)
                .AddApplication()
                .AddWebAPI();


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();
            app.UseCors("AllowAll");
            // Middleware để expose endpoint /metrics
            app.UseMetricServer();


            // Middleware để tự động đo thời gian và đếm các HTTP request
            app.UseHttpMetrics();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            //}

            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseMiddleware<JwtMiddleware>();
            app.UseMiddleware<JwtBlacklistMiddleware>();

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }
            app.Run();
        }
    }
}