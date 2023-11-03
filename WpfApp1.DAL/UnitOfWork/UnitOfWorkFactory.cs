using Microsoft.EntityFrameworkCore;

namespace WpfApp1.DAL.UnitOfWork;

public class UnitOfWorkFactory : IUnitOfWorkFactory
{
    private readonly IDbContextFactory<ProjectDbContext> _dbContextFactory;

    public UnitOfWorkFactory(IDbContextFactory<ProjectDbContext> dbContextFactory) =>
        _dbContextFactory = dbContextFactory;

    public IUnitOfWork Create() => new UnitOfWork(_dbContextFactory.CreateDbContext());
}