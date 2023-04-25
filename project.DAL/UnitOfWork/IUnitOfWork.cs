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
    public interface IUnitOfWork : IAsyncDisposable
    {
        IRepository<TEntity> GetRepository<TEntity, TEntityMapper>()
            where TEntity : class, IEntityID
            where TEntityMapper : IEntityIDMapper<TEntity>, new();

        Task CommitAsync();
    }
}
