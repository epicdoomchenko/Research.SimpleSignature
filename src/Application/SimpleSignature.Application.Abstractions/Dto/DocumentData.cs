using SimpleSignature.Domain.Entities;
using SimpleSignature.Domain.Enums;

namespace SimpleSignature.Application.Abstractions.Dto;

public class DocumentData
{
    public Guid Id { get; }
    public string Filename { get; }
    public DateTimeOffset Created { get; }
    public DateTimeOffset Signed{ get; }
    public SigningStatus SigningStatus { get; }

    public DocumentData(UserDocument userDocument)
    {
        Id = userDocument.DocumentId;
        Filename = userDocument.Document.Url.ToString();
        Created = userDocument.CreatedOn;
        Signed = userDocument.SignedUpOn;
        SigningStatus = userDocument.SigningStatus;
    }
}