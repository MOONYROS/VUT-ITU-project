using Microsoft.EntityFrameworkCore;
using projectk.DAL.Repositories;
using project.DAL.Repositories;
using project.DAL.Entities;
using project.DAL.Mappers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System;

namespace project.DAL.UnitOfWork
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;

        public UnitOfWork(DbContext dbContext) =>
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        public IRepository<TEntity> GetRepository<TEntity, TEntityMapper>()
            where TEntity : class, IEntityID
            where TEntityMapper : IEntityIDMapper<TEntity>, new()
            => new Repository<TEntity>(_dbContext, new TEntityMapper());

        public async Task CommitAsync() => await _dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync() => await _dbContext.DisposeAsync();
    }
}
