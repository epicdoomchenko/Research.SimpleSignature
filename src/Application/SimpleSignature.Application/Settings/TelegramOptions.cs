namespace SimpleSignature.Application.Settings;

public class TelegramOptions
{
    public string BotToken { get; set; }
    public string BotUserName { get; set; }
    public Uri WebhookUrl { get; set; }
}