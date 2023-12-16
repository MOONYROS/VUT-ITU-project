﻿using Microsoft.EntityFrameworkCore;
using WpfApp1.BL.Mappers.Interfaces;
using WpfApp1.BL.Models;
using WpfApp1.DAL.Entities;
using WpfApp1.DAL.Mappers;
using WpfApp1.DAL.Repositories;
using WpfApp1.DAL.UnitOfWork;

namespace WpfApp1.BL.Facades;

public class FacadeBaseDetailOnly<TEntity, TDetailModel, TEntityMapper>
    where TEntity : class, IEntityId
    where TDetailModel : class, IModel
    where TEntityMapper : IEntityIDMapper<TEntity>, new()
{
	private readonly IModelMapperDetailOnly<TEntity, TDetailModel> _modelMapper;
    protected readonly IUnitOfWorkFactory UnitOfWorkFactory;

    protected FacadeBaseDetailOnly(
        IUnitOfWorkFactory unitOfWorkFactory,
        IModelMapperDetailOnly<TEntity, TDetailModel> modelMapper)
    {
        UnitOfWorkFactory = unitOfWorkFactory;
        _modelMapper = modelMapper;
    }

    public async Task DeleteAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        try
        {
            uow.GetRepository<TEntity, TEntityMapper>().Delete(id);
            await uow.CommitAsync().ConfigureAwait(false);
        }
        catch (DbUpdateException e)
        {
            throw new InvalidOperationException("Entity deletion failed.", e);
        }
    }

    public virtual async Task<TDetailModel?> GetAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        TEntity? entity = await uow.GetRepository<TEntity, TEntityMapper>()
	        .Get()
	        .SingleOrDefaultAsync(e => e.Id == id);

        return entity is null
            ? null
            : _modelMapper.MapToDetailModel(entity);
    }

    public virtual async Task<TDetailModel> SaveAsync(TDetailModel model, Guid userId)
    {
        TDetailModel result;

        TEntity entity = _modelMapper.MapToEntity(model, userId);

        IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<TEntity> repository = uow.GetRepository<TEntity, TEntityMapper>();

        if (await repository.ExistsAsync(entity))
        {
            TEntity updatedEntity = await repository.UpdateAsync(entity);
            result = _modelMapper.MapToDetailModel(updatedEntity);
        }
        else
        {
            entity.Id = Guid.NewGuid();
            TEntity insertedEntity = await repository.InsertAsync(entity);
            result = _modelMapper.MapToDetailModel(insertedEntity);
        }

        await uow.CommitAsync();

        return result;
    }
}