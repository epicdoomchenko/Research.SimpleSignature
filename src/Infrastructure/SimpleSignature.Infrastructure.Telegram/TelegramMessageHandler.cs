using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Logging;
using SimpleSignature.Application.Abstractions.Dto;
using SimpleSignature.Application.Abstractions.Services;
using SimpleSignature.Application.Commands;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SimpleSignature.Infrastructure.Telegram;

public class TelegramMessageHandler(
    ILogger<TelegramMessageHandler> logger,
    TelegramBotClient telegramBotClient,
    IMediator mediator)
    : IUpdateHandler, INotificationSender
{
    public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source,
        CancellationToken cancellationToken)
    {
        logger.LogWarning("HandleError: {Exception}", exception);
        if (exception is RequestException)
        {
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        }
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await (update switch
        {
            { Message: { } message } => OnMessage(message, cancellationToken),
            { EditedMessage: { } message } => OnMessage(message, cancellationToken),
            { CallbackQuery: { } callbackQuery } => OnCallbackQuery(callbackQuery, cancellationToken),
            // { InlineQuery: { } inlineQuery } => OnInlineQuery(inlineQuery),
            // { ChosenInlineResult: { } chosenInlineResult } => OnChosenInlineResult(chosenInlineResult),
            // { Poll: { } poll } => OnPoll(poll),
            // { PollAnswer: { } pollAnswer } => OnPollAnswer(pollAnswer),
            // ChannelPost:
            // EditedChannelPost:
            // ShippingQuery:
            // PreCheckoutQuery:
            _ => UnknownUpdateHandlerAsync(update)
        });
    }

    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await (update switch
        {
            { Message: { } message } => OnMessage(message, cancellationToken),
            { EditedMessage: { } message } => OnMessage(message, cancellationToken),
            { CallbackQuery: { } callbackQuery } => OnCallbackQuery(callbackQuery, cancellationToken),
            // { InlineQuery: { } inlineQuery } => OnInlineQuery(inlineQuery),
            // { ChosenInlineResult: { } chosenInlineResult } => OnChosenInlineResult(chosenInlineResult),
            // { Poll: { } poll } => OnPoll(poll),
            // { PollAnswer: { } pollAnswer } => OnPollAnswer(pollAnswer),
            // ChannelPost:
            // EditedChannelPost:
            // ShippingQuery:
            // PreCheckoutQuery:
            _ => UnknownUpdateHandlerAsync(update)
        });
    }

    public async Task SendAsync(long chatId, string message, CancellationToken cancellationToken = default)
    {
        await telegramBotClient.SendMessage(chatId, message, cancellationToken: cancellationToken);
    }

    public async Task SendFileAsync(long chatId, Stream fileStream, InlineButtonData[] inlineButtonData,
        CancellationToken cancellationToken = default)
    {
        var list = new List<InlineKeyboardButton>();
        foreach (var ibd in inlineButtonData)
        {
            list.Add(InlineKeyboardButton.WithCallbackData(ibd.Text, ibd.CallbackData));
        }

        var inlineKeyboard = new InlineKeyboardMarkup(
            new List<InlineKeyboardButton[]>
            {
                list.ToArray()
            });

        await telegramBotClient.SendDocument(chatId, new InputFileStream(fileStream),
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    }

    private async Task OnMessage(Message msg, CancellationToken cancellationToken)
    {
        logger.LogInformation("Receive message type: {MessageType}", msg.Type);

        if (msg.From == null)
        {
            return;
        }

        await mediator.Send(new CreateUser(msg.From.Id, msg.From.Username ?? string.Empty, msg.Chat.Id),
            cancellationToken);

        if (msg.Text is not { } messageText)
            return;

        var sentMessage = await (messageText.Split(' ')[0] switch
        {
            _ => Usage(msg)
        });
        logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.Id);
    }

    private async Task<Message> Usage(Message msg)
    {
        const string usage = """
                                 <b><u>Bot description</u></b>:
                                 Demo application for ASP.NET Core and Telegram.Bot
                             """;
        return await telegramBotClient.SendMessage(msg.Chat, usage, parseMode: ParseMode.Html,
            replyMarkup: new ReplyKeyboardRemove());
    }

    // Process Inline Keyboard callback data
    private async Task OnCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        logger.LogInformation("Received inline keyboard callback from: {CallbackQueryId}", callbackQuery.Id);
        var callbackData = JsonSerializer.Deserialize<CallbackInfo>(callbackQuery.Data!);
        if (callbackData == null)
        {
            return;
        }

        await mediator.Send(new SetSignStatus(callbackQuery.From.Id, callbackData.DocId, callbackData.Status),
            cancellationToken);
        try
        {
            await telegramBotClient.EditMessageReplyMarkup(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message!.MessageId,
                replyMarkup: null,
                cancellationToken: cancellationToken
            );
        }
        catch (Exception e)
        {
            logger.LogWarning(e, "Error while editing message");
        }

        await telegramBotClient.SendMessage(callbackQuery.Message!.Chat, "Result saved",
            cancellationToken: cancellationToken);
    }

    private Task UnknownUpdateHandlerAsync(Update update)
    {
        logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
        return Task.CompletedTask;
    }
}