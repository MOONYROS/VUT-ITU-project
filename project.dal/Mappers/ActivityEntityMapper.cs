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
            existingEntity.DateTimeFrom = newEntity.DateTimeFrom;
            existingEntity.DateTimeTo = newEntity.DateTimeTo;
            existingEntity.Name = newEntity.Name;
            existingEntity.Description = newEntity.Description;
            existingEntity.Color = newEntity.Color;
        }
    }
}

