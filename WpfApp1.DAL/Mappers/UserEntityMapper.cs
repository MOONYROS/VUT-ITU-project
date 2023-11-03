using WpfApp1.DAL.Entities;

namespace WpfApp1.DAL.Mappers;

public class UserEntityMapper : IEntityIDMapper<UserEntity>
{
    public void MapToExistingEntity(UserEntity existingEntity, UserEntity newEntity)
    {
        existingEntity.FullName= newEntity.FullName;
        existingEntity.UserName= newEntity.UserName;
        existingEntity.ImageUrl= newEntity.ImageUrl;
    }
}