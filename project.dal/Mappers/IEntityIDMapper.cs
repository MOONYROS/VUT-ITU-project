using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

using project.DAL.Entities;

namespace project.DAL.Mappers
{
    public interface IEntityIDMapper<in TEntity>
        where TEntity : IEntityID
    {
        void MapToExistingEntity(TEntity existingEntity, TEntity newEntity);
    }
}

