using project.BL.Mappers.Interfaces;
using project.DAL.Entities;

namespace project.BL.Mappers;

public abstract class ModelMapperBaseDetailOnly<TEntity, TDetailModel> 
    : IModelMapperDetailOnly<TEntity, TDetailModel>
{
    public abstract TDetailModel MapToDetailModel(TEntity entity);
    public abstract TEntity MapToEntity(TDetailModel model);
    public abstract TEntity MapToEntity(TDetailModel model, Guid userGuid);
}