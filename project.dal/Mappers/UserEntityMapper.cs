using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using project.DAL.Entities;

namespace project.DAL.Mappers
{
    public class UserEntityMapper : IEntityIDMapper<UserEntity>
    {
        public void MapToExistingEntity(UserEntity existingEntity, UserEntity newEntity)
        {
            existingEntity.FullName= newEntity.FullName;
            existingEntity.UserName= newEntity.UserName;
            existingEntity.ImageUrl= newEntity.ImageUrl;
        }
    }
}
