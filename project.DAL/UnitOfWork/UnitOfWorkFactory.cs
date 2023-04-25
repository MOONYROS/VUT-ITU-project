using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System;

namespace project.DAL.UnitOfWork
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IDbContextFactory<ProjectDbContext> _dbContextFactory;

        public UnitOfWorkFactory(IDbContextFactory<ProjectDbContext> dbContextFactory) =>
            _dbContextFactory = dbContextFactory;

        public IUnitOfWork Create() => new UnitOfWork(_dbContextFactory.CreateDbContext());
    }
}
