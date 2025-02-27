using SimpleSignature.Application.Abstractions.Dto;

namespace SimpleSignature.Application.Abstractions.Services;

public interface INotificationSender
{
    Task SendAsync(long chatId, string message, CancellationToken cancellationToken = default);
    Task SendFileAsync(long chatId, Stream fileStream, InlineButtonData[] options, CancellationToken cancellationToken = default);
}