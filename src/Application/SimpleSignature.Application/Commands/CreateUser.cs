using MediatR;
using SimpleSignature.Application.Abstractions.Repositories;
using SimpleSignature.Domain.Entities;

namespace SimpleSignature.Application.Commands;

public class CreateUser(long id, string username, long chatId = -1) : IRequest
{
    public long Id { get; } = id;
    public string Username { get; } = username;
    
    public long ChatId { get; } = chatId;
}

internal sealed class CreateUserHandler(IUserRepository userRepository) : IRequestHandler<CreateUser>
{
    public async Task Handle(CreateUser request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user != null)
        {
            if (user.ChatId == -1 && request.ChatId != -1)
            {
                user.SetUserChatId(request.ChatId);
                await userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }
            return;
        }

        userRepository.Add(new User(request.Id, request.Username, request.ChatId));
        await userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}