using MediatR;
using SimpleSignature.Application.Abstractions.Dto;
using SimpleSignature.Application.Abstractions.Repositories;

namespace SimpleSignature.Application.Queries;

public class GetUserDocuments(long userId) : IRequest<IReadOnlyCollection<UserDocumentData>>
{
    public long UserId { get; } = userId;
}

public class GetUserDocumentsHandler(IUserRepository userRepository)
    : IRequestHandler<GetUserDocuments, IReadOnlyCollection<UserDocumentData>>
{
    public async Task<IReadOnlyCollection<UserDocumentData>> Handle(GetUserDocuments request, CancellationToken cancellationToken)
    {
        var userDocuments =await userRepository.GetUserDocumentsDocumentsAsync(request.UserId, cancellationToken);
        return userDocuments.Select(ud => new UserDocumentData(ud)).ToList().AsReadOnly();
    }
}