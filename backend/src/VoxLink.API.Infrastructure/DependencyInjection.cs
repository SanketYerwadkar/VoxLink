using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VoxLink.API.Application.Common;
using VoxLink.API.Infrastructure.Data;

namespace VoxLink.API.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        // Custom password hashing
        services.AddScoped<IPasswordHasher<VoxLink.API.Domain.Entities.User>, PasswordHasher<VoxLink.API.Domain.Entities.User>>();

        return services;
    }
}
