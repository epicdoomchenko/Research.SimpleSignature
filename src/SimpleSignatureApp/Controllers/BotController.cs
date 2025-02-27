using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SimpleSignature.Application.Settings;
using SimpleSignature.Infrastructure.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SimpleSignatureApp.Controllers;

[ApiController]
[Route("[controller]")]
public class BotController(IOptions<TelegramOptions> config) : ControllerBase
{
    [HttpGet("setWebhook")]
    public async Task<string> SetWebHook([FromServices] ITelegramBotClient bot, CancellationToken ct)
    {
        var webhookUrl = config.Value.WebhookUrl.AbsoluteUri;
        await bot.SetWebhook(webhookUrl, allowedUpdates: [], secretToken: config.Value.BotToken, cancellationToken: ct);
        return $"Webhook set to {webhookUrl}";
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update, [FromServices] ITelegramBotClient bot, [FromServices] UpdateHandler handleUpdateService, CancellationToken ct)
    {
        if (Request.Headers["X-Telegram-Bot-Api-Secret-Token"] != config.Value.BotToken)
            return Forbid();
        try
        {
            await handleUpdateService.HandleUpdateAsync(bot, update, ct);
        }
        catch (Exception exception)
        {
            await handleUpdateService.HandleErrorAsync(bot, exception, Telegram.Bot.Polling.HandleErrorSource.HandleUpdateError, ct);
        }
        return Ok();
    }
}