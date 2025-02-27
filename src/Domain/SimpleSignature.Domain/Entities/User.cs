using SimpleSignature.Domain.Enums;

namespace SimpleSignature.Domain.Entities;

public class User
{
    public long Id { get; private set; }
    public string Userame { get; private set; }

    public ICollection<UserDocument> Documents { get; private set; } = new List<UserDocument>();

    private User()
    {
    }

    public User(long id, string userame)
    {
        Id = id;
        Userame = userame;
    }

    public void AddDocument(UserDocument document)
    {
        Documents.Add(document);
    }

    public void SetDocumentStatus(Guid documentId, SigningStatus signingStatus)
    {
        Documents.Single(s => s.DocumentId == documentId).SetSignedUpOn(signingStatus);
    }
}