using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;
using project.DAL.Mappers;
using project.DAL.UnitOfWork;

namespace project.BL.Facades;

public class FacadeBaseDetailOnly<TEntity, TDetailModel, TEntityMapper>
    where TEntity : class, IEntityID
    where TDetailModel : class, IModel
    where TEntityMapper : IEntityIDMapper<TEntity>, new()
{
    protected FacadeBaseDetailOnly(
        IUnitOfWorkFactory unitOfWorkFactory,
        IModelMapperDetailOnly<TEntity, TDetailModel> modelMapper)
    {
        throw new NotImplementedException();
    }
}