using project.DAL.Entities;

namespace project.BL.Facades.Interfaces;

public interface IFacadeBindingEntity<TEntity>
    where TEntity : class, IEntityID
{
    
}