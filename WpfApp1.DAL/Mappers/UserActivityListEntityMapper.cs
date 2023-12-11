using WpfApp1.DAL.Entities;

namespace WpfApp1.DAL.Mappers;

public class UserActivityListEntityMapper : IEntityIDMapper<UserActivityListEntity>
{
	public void MapToExistingEntity(UserActivityListEntity existingEntity, UserActivityListEntity newEntity)
	{
		existingEntity.ActivityId = newEntity.ActivityId;
		existingEntity.UserId = newEntity.UserId;
	}
}