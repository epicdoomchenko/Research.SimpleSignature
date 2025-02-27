using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleSignature.Domain.Entities;

namespace SimpleSignature.Infrastructure.DAL.EntityConfiguration;

public class UserDocumentEntityConfiguration : IEntityTypeConfiguration<UserDocument>
{
    public void Configure(EntityTypeBuilder<UserDocument> builder)
    {
        builder.HasKey(ud => new { ud.UserId, ud.DocumentId });
    }
}