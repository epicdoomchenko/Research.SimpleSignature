using MediatR;
using SimpleSignature.Application.Abstractions.Repositories;
using SimpleSignature.Domain.Entities;

namespace SimpleSignature.Application.Commands;

public class CreateSigningDocument(Guid documentId, long userId) : IRequest
{
    public Guid DocumentId { get; set; } = documentId;
    public long UserId { get; set; } = userId;
}

internal sealed class CreateSigningDocumentHandler(IUserRepository userRepository)
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
    }
}