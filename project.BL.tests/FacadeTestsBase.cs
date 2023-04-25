using Microsoft.EntityFrameworkCore;
using project.BL.Mappers;
using project.DAL;
using project.DAL.Mappers;
using project.DAL.Tests;
using project.DAL.UnitOfWork;

namespace project.BL.tests;

public class FacadeTestsBase : IAsyncLifetime
{
    protected FacadeTestsBase()
    {

        DbContextFactory = new DbContextSqLiteTestingFactory(GetType().FullName!);

        ActivityEntityMapper = new ActivityEntityMapper();
        ActivityTagListEntityMapper = new ActivityTagListEntityMapper();
        ProjectEntityMapper = new ProjectEntityMapper();
        TagEntityMapper = new TagEntityMapper();
        TodoEntityMapper = new TodoEntityMapper();
        UserEntityMapper = new UserEntityMapper();
        UserProjectListEntityMapper = new UserProjectListEntityMapper();

        ActivityModelMapper = new ActivityModelMapper();
        ActivityTagModelMapper = new ActivityTagModelMapper(); 
        ProjectModelMapper = new ProjectModelMapper();
        TagModelMapper = new TagModelMapper();
        TodoModelMapper = new TodoModelMapper();
        UserModelMapper = new UserModelMapper();
        UserProjectModelMapper = new UserProjectModelMapper();

        UnitOfWorkFactory = new UnitOfWorkFactory(DbContextFactory);
    }

    protected IDbContextFactory<ProjectDbContext> DbContextFactory { get; }

    protected ActivityEntityMapper ActivityEntityMapper { get; }
    protected UserEntityMapper UserEntityMapper { get; }
    protected ProjectEntityMapper ProjectEntityMapper { get; }
    protected ActivityTagListEntityMapper ActivityTagListEntityMapper { get; }
    protected TagEntityMapper TagEntityMapper { get; }
    protected TodoEntityMapper TodoEntityMapper { get; }
    protected UserProjectListEntityMapper UserProjectListEntityMapper { get; }


    protected UserProjectModelMapper UserProjectModelMapper { get; }
    protected TagModelMapper TagModelMapper { get; }
    protected ActivityTagModelMapper ActivityTagModelMapper { get; }
    protected ActivityModelMapper ActivityModelMapper { get; }
    protected ProjectModelMapper ProjectModelMapper { get; } 
    protected TodoModelMapper TodoModelMapper { get; }
    protected UserModelMapper UserModelMapper { get; } 

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