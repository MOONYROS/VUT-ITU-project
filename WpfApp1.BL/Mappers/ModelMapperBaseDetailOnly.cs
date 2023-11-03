using Microsoft.EntityFrameworkCore.ChangeTracking;
using WpfApp1.BL.Mappers.Interfaces;
using WpfApp1.DAL.Entities;

namespace WpfApp1.BL.Mappers;

public abstract class ModelMapperBaseDetailOnly<TEntity, TDetailModel> 
    : IModelMapperDetailOnly<TEntity, TDetailModel>
{
    public abstract TDetailModel MapToDetailModel(TEntity? entity);
    public abstract TEntity MapToEntity(TDetailModel model);
    public abstract TEntity MapToEntity(TDetailModel model, Guid userGuid);

    public IEnumerable<TDetailModel> MapToDetailModel(IEnumerable<TEntity> entities)
        => entities.Select(MapToDetailModel);
}