namespace project.BL.Mappers.Interfaces;

public interface IModelMapper<TEntity, out TListModel, TDetailModel>
{
    TListModel MapToListModel(TEntity? entity);
    TDetailModel MapToDetailModel(TEntity entity);
    TEntity MapToEntity(TDetailModel model);
    TEntity MapToEntity(TDetailModel model, Guid guid);
    IEnumerable<TListModel> MapToListModel(IEnumerable<TEntity> entities)
        => entities.Select(MapToListModel);
}