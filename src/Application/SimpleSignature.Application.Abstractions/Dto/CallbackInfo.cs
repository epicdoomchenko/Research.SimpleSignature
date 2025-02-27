using SimpleSignature.Domain.Enums;

namespace SimpleSignature.Application.Abstractions.Dto;

public class CallbackInfo
{
    public SigningStatus Status { get; init; }
    public Guid DocId { get; init;  }
}