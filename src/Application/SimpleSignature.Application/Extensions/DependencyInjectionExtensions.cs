using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleSignature.Application.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config => { config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); });

        return services;
    }
}