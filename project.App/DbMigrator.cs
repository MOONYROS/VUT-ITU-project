using Microsoft.EntityFrameworkCore;
using project.DAL;
using project.App.Options;

namespace project.App;

public interface IDbMigrator
{
    public void Migrate();
    public Task MigrateAsync(CancellationToken cancellationToken);
}

public class SqliteDbMigrator : IDbMigrator
{
    private readonly IDbContextFactory<ProjectDbContext> _dbContextFactory;
    private readonly SqliteOptions _sqliteOptions;

    public SqliteDbMigrator(IDbContextFactory<ProjectDbContext> dbContextFactory, DALOptions dalOptions)
    {
        _dbContextFactory = dbContextFactory;
        _sqliteOptions = dalOptions.Sqlite ?? throw new ArgumentNullException(nameof(dalOptions), $@"{nameof(DALOptions.Sqlite)} are not set");
    }

    public void Migrate() => MigrateAsync(CancellationToken.None).GetAwaiter().GetResult();

    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        await using ProjectDbContext dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        if (_sqliteOptions.RecreateDatabaseEachTime)
        {
            await dbContext.Database.EnsureDeletedAsync(cancellationToken);
        }

        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
    }
}

