using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using project.DAL.Entities;

namespace project.DAL.Mappers
{
    public class UserProjectListEntityMapper : IEntityIDMapper<UserProjectListEntity>
    {
        public void MapToExistingEntity(UserProjectListEntity existingEntity, UserProjectListEntity newEntity)
        {
            existingEntity.UserId= newEntity.UserId;
            existingEntity.ProjectId= newEntity.ProjectId;
        }
    }
}
