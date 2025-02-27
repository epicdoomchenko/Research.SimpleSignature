using System.Text.Json;
using MediatR;
using SimpleSignature.Application.Abstractions.Dto;
using SimpleSignature.Application.Abstractions.Repositories;
using SimpleSignature.Application.Abstractions.Services;
using SimpleSignature.Domain.Entities;
using SimpleSignature.Domain.Enums;

namespace SimpleSignature.Application.Commands;

public class CreateSigningDocument(Guid documentId, long userId) : IRequest
{
    public Guid DocumentId { get; } = documentId;
    public long UserId { get; } = userId;
}

internal sealed class CreateSigningDocumentHandler(
    IUserRepository userRepository,
    IDocumentRepository documentRepository,
    INotificationSender notificationSender)
    : IRequestHandler<CreateSigningDocument>
{
    public async Task Handle(CreateSigningDocument request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            throw new Exception();
        }

        var userDocument = new UserDocument(request.DocumentId, request.UserId);
        user.AddDocument(userDocument);
        await userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);


        var document = await documentRepository.GetByDocumentIdAsync(request.DocumentId, cancellationToken);
        var fileReader = File.OpenRead(document.Url.AbsolutePath);

        var optionAccess = JsonSerializer.Serialize(new CallbackInfo
        {
            DocId = request.DocumentId,
            Status = SigningStatus.Access
        });
        
        var optionDeny = JsonSerializer.Serialize(new CallbackInfo
        {
            DocId = request.DocumentId,
            Status = SigningStatus.Deny
        });

        InlineButtonData[] options = [new("Sign", optionAccess), new("Cancel", optionDeny)];
        await notificationSender.SendFileAsync(user.ChatId, fileReader, options, cancellationToken);
    }
}