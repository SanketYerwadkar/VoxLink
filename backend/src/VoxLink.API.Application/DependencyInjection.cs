using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System.Reflection;
using MediatR;

namespace VoxLink.API.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
