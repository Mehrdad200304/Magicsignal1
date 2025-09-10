using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MagicSignal.Modules.Accounts.Infrastructure.Persistence;

namespace MagicSignal.Modules.Accounts.Infrastructure;

public class DesignTimeApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        optionsBuilder.UseSqlServer("Server=localhost;Database=MagicSignalDb;Trusted_Connection=True;TrustServerCertificate=True;");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}