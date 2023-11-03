using WpfApp1.DAL.Entities;

namespace WpfApp1.DAL.Mappers;

public interface IEntityIDMapper<in TEntity>
    where TEntity : IEntityID
{
    void MapToExistingEntity(TEntity existingEntity, TEntity newEntity);
}