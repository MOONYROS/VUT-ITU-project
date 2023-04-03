using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using project.DAL.Entities;

namespace project.DAL.Mappers
{
    public class TagEntityMapper : IEntityIDMapper<TagEntity>
    {
        public void MapToExistingEntity(TagEntity existingEntity, TagEntity newEntity)
        {
            existingEntity.Name = newEntity.Name;
            existingEntity.Color= newEntity.Color;
        }
    }
}
