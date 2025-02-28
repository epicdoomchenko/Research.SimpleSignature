using MediatR;
using SimpleSignature.Application.Abstractions.Dto;
using SimpleSignature.Application.Abstractions.Repositories;

namespace SimpleSignature.Application.Queries;

public class GetUsers : IRequest<IReadOnlyCollection<UserLongData>>
{
    
}

internal class GetUsersHandler(IUserRepository userRepository)
    : IRequestHandler<GetUsers, IReadOnlyCollection<UserLongData>>
{
    public async Task<IReadOnlyCollection<UserLongData>> Handle(GetUsers request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllAsync(cancellationToken);
        return users.Select(u => new UserLongData
        {
            Id = u.Id,
            Userame = u.Username,
            IsActive = u.ChatId != -1
        }).ToList().AsReadOnly();
    }
}