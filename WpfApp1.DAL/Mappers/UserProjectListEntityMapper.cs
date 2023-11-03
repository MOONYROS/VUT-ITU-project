using WpfApp1.DAL.Entities;

namespace WpfApp1.DAL.Mappers;

public class UserProjectListEntityMapper : IEntityIDMapper<UserProjectListEntity>
{
    public void MapToExistingEntity(UserProjectListEntity existingEntity, UserProjectListEntity newEntity)
    {
        existingEntity.UserId= newEntity.UserId;
        existingEntity.ProjectId= newEntity.ProjectId;
    }
}