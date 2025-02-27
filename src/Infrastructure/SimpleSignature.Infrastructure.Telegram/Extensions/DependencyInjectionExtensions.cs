using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SimpleSignature.Application.Settings;
using Telegram.Bot;

namespace SimpleSignature.Infrastructure.Telegram.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddTelegramServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TelegramOptions>(configuration.GetSection(nameof(TelegramOptions)));
        var token = configuration.GetValue<string>($"{nameof(TelegramOptions)}:{nameof(TelegramOptions.BotToken)}");
        ArgumentNullException.ThrowIfNull(token);
        services.ConfigureTelegramBot<Microsoft.AspNetCore.Http.Json.JsonOptions>(opt => opt.SerializerOptions);
        services.AddHttpClient("tgwebhook").RemoveAllLoggers()
            .AddTypedClient(httpClient => new TelegramBotClient(token, httpClient));

        services.AddScoped<UpdateHandler>();

        return services;
    }
}