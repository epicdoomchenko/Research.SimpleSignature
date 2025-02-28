using MediatR;
using SimpleSignature.Application.Abstractions.Dto;
using SimpleSignature.Application.Abstractions.Repositories;

namespace SimpleSignature.Application.Queries;

public class GerActiveUsers : IRequest<IReadOnlyCollection<UserData>>
{
}

internal class GerActiveUsersHandler : IRequestHandler<GerActiveUsers, IReadOnlyCollection<UserData>>
{
    private readonly IUserRepository _repository;

    public GerActiveUsersHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<UserData>> Handle(GerActiveUsers request, CancellationToken cancellationToken)
    {
        var users = await _repository.GetActivatedUsersAsync(cancellationToken);
        return users.Select(c => new UserData { Id = c.Id, Userame = c.Username }).ToList().AsReadOnly();
    }
}