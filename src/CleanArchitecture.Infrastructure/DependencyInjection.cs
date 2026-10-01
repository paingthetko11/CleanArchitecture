using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Persistence;
using CleanArchitecture.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not configured.");
        services.AddScoped<AuditableEntityInterceptor>();
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            options.UseSqlite(connectionString).AddInterceptors(serviceProvider.GetRequiredService<AuditableEntityInterceptor>()));
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        return services;
    }
}
