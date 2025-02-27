using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SimpleSignature.Infrastructure.DAL;

public class AppDesignTimeDbContext : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionBuilder.UseSqlite();
        DbContextOptions<AppDbContext> options = optionBuilder.Options;
        return new AppDbContext(options);
    }
}