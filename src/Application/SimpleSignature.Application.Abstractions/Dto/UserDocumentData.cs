using SimpleSignature.Domain.Entities;
using SimpleSignature.Domain.Enums;

namespace SimpleSignature.Application.Abstractions.Dto;

public class UserDocumentData
{
    public Guid Id { get; }
    public string Filename { get; }
    public DateTimeOffset Created { get; }
    public DateTimeOffset Signed{ get; }
    public SigningStatus SigningStatus { get; }

    public UserDocumentData(UserDocument userDocument)
    {
        Id = userDocument.DocumentId;
        Filename = userDocument.Document.Url.ToString();
        Created = userDocument.CreatedOn;
        Signed = userDocument.SignedUpOn;
        SigningStatus = userDocument.SigningStatus;
    }
}