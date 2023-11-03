using WpfApp1.DAL.Entities;

namespace WpfApp1.DAL.Mappers;

public class TagEntityMapper : IEntityIDMapper<TagEntity>
{
    public void MapToExistingEntity(TagEntity existingEntity, TagEntity newEntity)
    {
        existingEntity.Name = newEntity.Name;
        existingEntity.Color= newEntity.Color;
    }
}