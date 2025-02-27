using Microsoft.Extensions.DependencyInjection;
using SimpleSignature.Application.Abstractions.Services;

namespace SimpleSignature.Infrastructure.FileStore.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddFileStore(this IServiceCollection services)
    {
        services.AddSingleton<IFileManager, FileManager>();
        return services;
    }
}