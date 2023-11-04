using Microsoft.EntityFrameworkCore;
using WpfApp1.DAL.Tests;
using WpfApp1.DAL.UnitOfWork;
using Xunit.Abstractions;
using WpfApp1.BL.Mappers;
using WpfApp1.BL.Mappers.Interfaces;
using WpfApp1.DAL;
using WpfApp1.DAL.Mappers;

namespace WpfApp1.BL.tests;

public class FacadeTestsBase : IAsyncLifetime
{
    protected FacadeTestsBase(ITestOutputHelper output)
    {
        TagModelMapper = new TagModelMapper();
        TodoModelMapper = new TodoModelMapper();
        UserModelMapper = new UserModelMapper();
        ActivityModelMapper = new ActivityModelMapper();

        ActivityEntityMapper = new ActivityEntityMapper();
        ActivityTagListEntityMapper = new ActivityTagListEntityMapper();
        TagEntityMapper = new TagEntityMapper();
        TodoEntityMapper = new TodoEntityMapper();
        UserEntityMapper = new UserEntityMapper();
        
        DbContextFactory = new DbContextSqLiteTestingFactory(GetType().FullName!);
        UnitOfWorkFactory = new UnitOfWorkFactory(DbContextFactory);
    }

    private IDbContextFactory<ProjectDbContext> DbContextFactory { get; }
    protected ActivityModelMapper ActivityModelMapper { get; }
    protected TagModelMapper TagModelMapper { get; }
    protected ITodoModelMapper TodoModelMapper { get; }
    protected UserModelMapper UserModelMapper { get; }

    protected ActivityEntityMapper ActivityEntityMapper { get; }
    protected ActivityTagListEntityMapper ActivityTagListEntityMapper { get; }
    protected TagEntityMapper TagEntityMapper { get; }
    protected TodoEntityMapper TodoEntityMapper { get; }
    protected UserEntityMapper UserEntityMapper { get; }
    protected UnitOfWorkFactory UnitOfWorkFactory { get; }

    public async Task InitializeAsync()
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        await dbx.Database.EnsureDeletedAsync();
        await dbx.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        await dbx.Database.EnsureDeletedAsync();
    }
}
