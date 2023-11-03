using WpfApp1.DAL.Entities;

namespace WpfApp1.DAL.Mappers;

public class ProjectEntityMapper : IEntityIDMapper<ProjectEntity>
{
    public void MapToExistingEntity(ProjectEntity existingEntity, ProjectEntity newEntity)
    {
        existingEntity.Name= newEntity.Name;
        existingEntity.Description= newEntity.Description;
    }
}