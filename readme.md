# SimpleSignature

SimpleSignature is a demo application for implementing Simple Electronic Signature (SES). It includes a Telegram bot and a Web UI built with Blazor.

## Usage

To create your own bot, start a conversation with [BotFather](https://t.me/BotFather) and follow the instructions.

After creating the bot, call the ``/setdomain`` command in BotFather, select your bot from the list, and send the public domain where your webhook API will be hosted.
The domain must be publicly accessible because Telegram sends updates to this address. If you are debugging locally, you can use tools like [ngrok](https://ngrok.com) to expose your local server to the internet.


Next, configure the following settings in `src/SimpleSignatureApp/appsettings.json`:
```json
{
  // other options
  "TelegramOptions": {
    "BotUsername": "your_bot_username",
    "BotToken": "your_bot_token",
    "WebhookUrl": "https://your-public-domain/api/webhook"
  }
}
```

Run the application using your preferred IDE or execute the following command in the terminal:
```bash
dotnet run --project src/SimpleSignatureApp
```

Finally, make an HTTP GET request to `[yourhost]/api/setWebhook` to complete the setup.