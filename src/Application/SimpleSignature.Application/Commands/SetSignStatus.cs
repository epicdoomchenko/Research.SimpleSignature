using MediatR;
using SimpleSignature.Application.Abstractions.Repositories;
using SimpleSignature.Domain.Enums;

namespace SimpleSignature.Application.Commands;

public class SetSignStatus(long userId, Guid documentId, SigningStatus signStatus) : IRequest
{
    public long UserId { get; } = userId;
    public Guid DocumentId { get; } = documentId;
    public SigningStatus SignStatus { get; } = signStatus;
}

internal sealed class SetSignStatusHandler(IUserRepository userRepository) : IRequestHandler<SetSignStatus>
{
    public async Task Handle(SetSignStatus request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            throw new Exception();
        }

        user.SetDocumentStatus(request.DocumentId, request.SignStatus);

        await userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}