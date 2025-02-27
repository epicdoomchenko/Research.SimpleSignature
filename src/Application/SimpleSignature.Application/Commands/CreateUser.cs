using MediatR;
using SimpleSignature.Application.Abstractions.Repositories;
using SimpleSignature.Domain.Entities;

namespace SimpleSignature.Application.Commands;

public class CreateUser(long id, string username) : IRequest
{
    public long Id { get; } = id;
    public string Username { get; } = username;
}

internal sealed class CreateUserHandler(IUserRepository userRepository) : IRequestHandler<CreateUser>
{
    public async Task Handle(CreateUser request, CancellationToken cancellationToken)
    {
        if (await userRepository.ExistsAsync(request.Id, cancellationToken))
        {
            return;
        }

        userRepository.Add(new User(request.Id, request.Username));
        await userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}