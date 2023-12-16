using Microsoft.EntityFrameworkCore;

namespace WpfApp1.DAL.Factories;

public class DbContextSqLiteFactory : IDbContextFactory<ProjectDbContext>
{
    private readonly bool _seedTestingData;
    private readonly DbContextOptionsBuilder<ProjectDbContext> _contextOptionBuilder = new();

    public DbContextSqLiteFactory(string databaseName, bool seedTestingData = false)
    {
        _seedTestingData = seedTestingData;
        _contextOptionBuilder.UseSqlite($"Data Source={databaseName};Cache=Shared");
    }

    public ProjectDbContext CreateDbContext() => new(_contextOptionBuilder.Options, _seedTestingData);
}