using SimpleSignature.Domain.Enums;

namespace SimpleSignature.Domain.Entities;

public class UserDocument
{
    public Guid DocumentId { get; private set; }
    public long UserId { get; private set; }
    public DateTimeOffset CreatedOn { get; private set; }
    public DateTimeOffset SignedUpOn { get; private set; }
    public SigningStatus SigningStatus { get; private set; }
    public User User { get; private set; }
    public Document Document { get; private set; }

    private UserDocument()
    {
    }

    public UserDocument(Guid documentId, long userId, DateTimeOffset createdOn, SigningStatus signingStatus)
    {
        DocumentId = documentId;
        UserId = userId;
        CreatedOn = createdOn;
        SigningStatus = signingStatus;
    }

    public UserDocument(Guid documentId, long userId)
    {
        DocumentId = documentId;
        UserId = userId;
        CreatedOn = DateTimeOffset.Now;
        SigningStatus = SigningStatus.None;
    }

    public void SetSignedUpOn(SigningStatus status)
    {
        SigningStatus = status;
        SignedUpOn = DateTimeOffset.Now;
    }
}