using project.BL.Facades.Interfaces;
using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;
using project.DAL.Mappers;
using project.DAL.UnitOfWork;

namespace project.BL.Facades;

public class UserProjectFacade :
    FacadeBase<UserProjectListEntity, UserProjectListModel, UserProjectDetailModel, UserProjectListEntityMapper>, IUserProjectFacade
{
    public UserProjectFacade(
        IUnitOfWorkFactory unitOfWorkFactory,
        IUserProjectModelMapper modelMapper)
        : base(unitOfWorkFactory, modelMapper)
    {
    }
    
}