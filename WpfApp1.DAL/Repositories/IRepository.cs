﻿using WpfApp1.DAL.Entities;

namespace WpfApp1.DAL.Repositories;

public interface IRepository<TEntity>
    where TEntity : class, IEntityID
{
    IQueryable<TEntity> Get();
    void Delete(Guid entityId);
    ValueTask<bool> ExistsAsync(TEntity entity);
    Task<TEntity> InsertAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
}