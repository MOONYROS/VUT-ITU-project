using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using project.DAL.Entities;
using project.DAL.Mappers;

namespace project.DAL.Mappers
{
    public class ActivityEntityMapper : IEntityIDMapper<ActivityEntity>
    {
        public void MapToExistingEntity(ActivityEntity existingEntity, ActivityEntity newEntity)
        {
            existingEntity.DateFrom = newEntity.DateFrom;
            existingEntity.DateTo = newEntity.DateTo;
            existingEntity.Description = newEntity.Description;
            existingEntity.Name = newEntity.Name;
            existingEntity.TimeFrom = newEntity.TimeFrom;
            existingEntity.TimeTo = newEntity.TimeTo;
        }
    }
}

