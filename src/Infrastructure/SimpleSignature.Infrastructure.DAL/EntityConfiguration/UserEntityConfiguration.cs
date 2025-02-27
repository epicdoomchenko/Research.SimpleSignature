using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleSignature.Domain.Entities;

namespace SimpleSignature.Infrastructure.DAL.EntityConfiguration;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasMany(u => u.Documents)
            .WithOne(ud => ud.User)
            .HasForeignKey(ud => ud.UserId);
    }
}