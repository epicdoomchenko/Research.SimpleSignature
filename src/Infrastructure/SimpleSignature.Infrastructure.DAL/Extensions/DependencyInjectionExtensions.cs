using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleSignature.Application.Abstractions;
using SimpleSignature.Application.Abstractions.Repositories;
using SimpleSignature.Infrastructure.DAL.Repositories;

namespace SimpleSignature.Infrastructure.DAL.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDal(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options => { options.UseSqlite("Data Source=./app.db"); });

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>())
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IDocumentRepository, DocumentRepository>();

        return services;
    }
}