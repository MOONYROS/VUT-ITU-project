using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using project.DAL.Entities;

namespace project.DAL.Mappers
{
    public class ProjectEntityMapper : IEntityIDMapper<ProjectEntity>
    {
        public void MapToExistingEntity(ProjectEntity existingEntity, ProjectEntity newEntity)
        {
            existingEntity.Name= newEntity.Name;
            existingEntity.Description= newEntity.Description;
        }
    }
}
