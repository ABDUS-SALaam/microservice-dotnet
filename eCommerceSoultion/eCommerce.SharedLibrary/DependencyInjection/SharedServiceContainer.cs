using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using eCommerce.SharedLibrary.Middleware;

namespace eCommerce.SharedLibrary.DependencyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedServices<TContext>(this IServiceCollection services,IConfiguration config,string fileName) where TContext : DbContext
        {
            // Add DbContext
            services.AddDbContext<TContext>(options=>options.UseNpgsql(config.GetConnectionString("eCommerceConnection"),pgSqlOption=>pgSqlOption.EnableRetryOnFailure()));

            // Add Serilog logging
            Log.Logger=new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(path: $"{fileName}-.text",
                restrictedToMinimumLevel:Serilog.Events.LogEventLevel.Information,
                outputTemplate:"{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {message:lj}{NewLine}{Excpetion}",
                rollingInterval:RollingInterval.Day)
                .CreateLogger();

            // Add JWT Authentication Scheme
            JWTAuthenticationScheme.AddAuthenticationScheme(services, config);

            return services;
        }

        public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder app)
        {
            // Add exception handler
            app.UseMiddleware<GloablException>();

            // Register middleware to block all outsider API calls
            app.UseMiddleware<ListenToOnlyApiGateway>();

            return app;
        }
    }
}
