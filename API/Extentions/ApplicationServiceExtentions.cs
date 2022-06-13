using Application;
using Application.Interfaces;
using Application.Mappings;
using Domain.Interfaces;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistance;

namespace API.Extentions
{
    public static class ApplicationServiceExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIv5", Version = "v1" });
            });

            services.AddCors(opts =>
            {
                opts.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:3000");
                });
            });

            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IAttendenceService, AttendenceService>();

            services.AddAutoMapper(typeof(ActivityMapping).Assembly);

            services.AddScoped<IUserAccessor, UserAccessor>();

            return services;
        }
    }
}
