using MediatR;
using SimpleSignature.Application.Abstractions.Dto;
using SimpleSignature.Application.Abstractions.Repositories;

namespace SimpleSignature.Application.Queries;

public class GetUserDocuments(long userId) : IRequest<IEnumerable<DocumentData>>
{
    public long UserId { get; } = userId;
}

public class GetUserDocumentsHandler(IUserRepository userRepository)
    : IRequestHandler<GetUserDocuments, IEnumerable<DocumentData>>
{
    public async Task<IEnumerable<DocumentData>> Handle(GetUserDocuments request, CancellationToken cancellationToken)
    {
        var userDocuments =await userRepository.GetUserDocumentsDocumentsAsync(request.UserId, cancellationToken);
        return userDocuments.Select(ud => new DocumentData(ud));
    }
}