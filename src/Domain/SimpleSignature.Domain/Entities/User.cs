using SimpleSignature.Domain.Enums;

namespace SimpleSignature.Domain.Entities;

public class User
{
    public long Id { get; private set; }
    public string Username { get; private set; }
    public long ChatId { get; private set; }

    public ICollection<UserDocument> Documents { get; private set; } = new List<UserDocument>();

    private User()
    {
    }

    public User(long id, string username)
    {
        Id = id;
        Username = username;
        ChatId = -1;
    }

    public User(long id, string username, long chatId)
    {
        Id = id;
        Username = username;
        ChatId = chatId;
    }

    public void SetUserChatId(long chatId)
    {
        ChatId = chatId;
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