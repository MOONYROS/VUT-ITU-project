﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WpfApp1.DAL;

namespace WpfApp1.App;

public interface IDbMigrator
{
	public void Migrate();
	public Task MigrateAsync(CancellationToken cancellationToken);
}

public class SqliteDbMigrator : IDbMigrator
{
	private readonly IDbContextFactory<ProjectDbContext> _dbContextFactory;
	public SqliteDbMigrator(IDbContextFactory<ProjectDbContext> dbContextFactory)
	{
		_dbContextFactory = dbContextFactory;
	}

	public void Migrate() => MigrateAsync(CancellationToken.None).GetAwaiter().GetResult();

	public async Task MigrateAsync(CancellationToken cancellationToken)
	{
		await using ProjectDbContext dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

		if (false)
		{
			await dbContext.Database.EnsureDeletedAsync(cancellationToken);
		}
		await dbContext.Database.EnsureCreatedAsync(cancellationToken);
	}
}