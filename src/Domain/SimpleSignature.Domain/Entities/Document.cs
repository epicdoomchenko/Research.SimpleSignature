namespace SimpleSignature.Domain.Entities;

public class Document
{
    public Guid Id { get; private set; }
    public Uri Url { get; private set; }
    public ICollection<UserDocument> UserDocuments { get; private set; } = new List<UserDocument>();

    private Document()
    {
    }

    public Document(Guid id, Uri url)
    {
        Id = id;
        Url = url;
    }
}