using System.Collections;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using project.BL.Facades.Interfaces;
using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;
using project.DAL.Mappers;
using project.DAL.Repositories;
using project.DAL.UnitOfWork;

namespace project.BL.Facades;

public abstract class
    FacadeBase<TEntity, TListModel, TDetailModel, TEntityMapper> : IFacade<TEntity, TListModel, TDetailModel>
    where TEntity : class, IEntityID
    where TListModel : IModel
    where TDetailModel : class, IModel
    where TEntityMapper : IEntityIDMapper<TEntity>, new()
{
    protected readonly IModelMapper<TEntity, TListModel, TDetailModel> ModelMapper;
    protected readonly IUnitOfWorkFactory UnitOfWorkFactory;

    protected FacadeBase(
        IUnitOfWorkFactory unitOfWorkFactory,
        IModelMapper<TEntity, TListModel, TDetailModel> modelMapper)
    {
        UnitOfWorkFactory = unitOfWorkFactory;
        ModelMapper = modelMapper;
    }

    protected virtual string IncludesNavigationPathDetail => string.Empty;

    public async Task DeleteAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        uow.GetRepository<TEntity, TEntityMapper>().Delete(id);
        await uow.CommitAsync().ConfigureAwait(false);
    }

    public virtual async Task<TDetailModel?> GetAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<TEntity> query = uow.GetRepository<TEntity, TEntityMapper>().Get();

        if (string.IsNullOrWhiteSpace(IncludesNavigationPathDetail) is false)
        {
            query = query.Include(IncludesNavigationPathDetail);
        }

        TEntity? entity = await query.SingleOrDefaultAsync(e => e.Id == id);

        return entity is null
            ? null
            : ModelMapper.MapToDetailModel(entity);
    }

    public virtual async Task<IEnumerable<TListModel>> GetAsync()
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<TEntity> entities = await uow
            .GetRepository<TEntity, TEntityMapper>()
            .Get()
            .ToListAsync();

        return ModelMapper.MapToListModel(entities);
    }

    public async Task<TDetailModel> SaveAsync(TDetailModel model, Guid id)
    {
        TDetailModel result;

        GuardCollectionsAreNotSet(model);

        TEntity entity = ModelMapper.MapToEntity(model, id);

        IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<TEntity> repository = uow.GetRepository<TEntity, TEntityMapper>();

        if (await repository.ExistsAsync(entity))
        {
            TEntity updatedEntity = await repository.UpdateAsync(entity);
            result = ModelMapper.MapToDetailModel(updatedEntity);
        }
        else
        {
            entity.Id = Guid.NewGuid();
            TEntity insertedEntity = await repository.InsertAsync(entity);
            result = ModelMapper.MapToDetailModel(insertedEntity);
        }

        await uow.CommitAsync();

        return result;
    }

    public virtual async Task<TDetailModel> SaveAsync(TDetailModel model)
    {
        TDetailModel result;

        GuardCollectionsAreNotSet(model);

        TEntity entity = ModelMapper.MapToEntity(model);

        IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<TEntity> repository = uow.GetRepository<TEntity, TEntityMapper>();

        if (await repository.ExistsAsync(entity))
        {
            TEntity updatedEntity = await repository.UpdateAsync(entity);
            result = ModelMapper.MapToDetailModel(updatedEntity);
        }
        else
        {
            entity.Id = Guid.NewGuid();
            TEntity insertedEntity = await repository.InsertAsync(entity);
            result = ModelMapper.MapToDetailModel(insertedEntity);
        }

        await uow.CommitAsync();

        return result;
    }

    public static void GuardCollectionsAreNotSet(TDetailModel model)
    {
        IEnumerable<PropertyInfo> collectionProperties = model
            .GetType()
            .GetProperties()
            .Where(i => typeof(ICollection).IsAssignableFrom(i.PropertyType));

        foreach (PropertyInfo collectionProperty in collectionProperties)
        {
            if (collectionProperty.GetValue(model) is ICollection { Count: > 0 })
            {
                throw new InvalidOperationException(
                    "Current BL and DAL infrastructure disallows insert or update of models with adjacent collections.");
            }
        }
    }


}