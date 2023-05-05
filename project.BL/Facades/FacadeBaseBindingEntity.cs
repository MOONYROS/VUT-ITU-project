using project.BL.Facades.Interfaces;
using project.DAL.Entities;

namespace project.BL.Facades;

public class FacadeBaseBindingEntity<TEntity> : IFacadeBindingEntity<TEntity> 
    where TEntity : class, IEntityID
{
    
}