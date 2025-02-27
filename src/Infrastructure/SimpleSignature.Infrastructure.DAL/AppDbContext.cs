using Microsoft.EntityFrameworkCore;
using SimpleSignature.Application.Abstractions;
using SimpleSignature.Domain.Entities;

namespace SimpleSignature.Infrastructure.DAL;

public class AppDbContext : DbContext, IUnitOfWork
{
    public DbSet<User> Users { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<UserDocument> UserDocuments { get; set; }

    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}