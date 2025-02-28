using SimpleSignature.Domain.Entities;

namespace SimpleSignature.Application.Abstractions.Dto;

public class DocumentData
{
    public Guid Id { get; }
    public string FileName { get; }

    public DocumentData(Document document)
    {
        Id = document.Id;
        FileName = document.Url.ToString();
    }
}
