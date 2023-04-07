namespace project.BL.Mappers.Interfaces;

public interface IModelMapper<TEntity, out TListModel, TDetailModel>
{
    TListModel MapToListModel(TEntity? entity);
    TDetailModel MapToDetailModel(TEntity entity);
    TEntity MapToEntity(TDetailModel model);
}