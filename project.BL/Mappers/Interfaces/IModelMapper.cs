using project.DAL.Entities;


namespace project.BL.Mappers.Interfaces;

public interface IModelMapper<TEntity, out TListModel, TDetailModel>
{
    TListModel MapToListModel(TEntity? entity);
    TDetailModel MapToDetailModel(TEntity entity);
    TEntity MapToEntity(TDetailModel model);
    IEnumerable<TListModel> MapToListModel(IEnumerable<TEntity> entities)
        => entities.Select(MapToListModel);
}