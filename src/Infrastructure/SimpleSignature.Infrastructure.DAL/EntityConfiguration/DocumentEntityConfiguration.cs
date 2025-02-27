using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleSignature.Domain.Entities;

namespace SimpleSignature.Infrastructure.DAL.EntityConfiguration;

public class DocumentEntityConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.HasMany(d => d.UserDocuments)
            .WithOne(u => u.Document)
            .HasForeignKey(u => u.DocumentId);
    }
}