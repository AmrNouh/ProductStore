using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Core.Models;

namespace ProductManagement.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ProductStoreDBContext>(options => options.UseSqlServer(connectionString));
        }

        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

                // allow requests only from that concrete source with only post and get method and the header must contain accept and content-type
                //builder.WithOrigins("https://example.com")
                //.WithMethods("POST","GET")
                //.WithHeaders("accept", "contenttype"));
            });
    }
}
