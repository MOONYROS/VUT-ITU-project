using project.BL.Facades.Interfaces;
using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;
using project.DAL.Mappers;
using project.DAL.UnitOfWork;

namespace project.BL.Facades;

public class ActivityTagFacade  :
    FacadeBase<ActivityTagListEntity, ActivityTagListModel, ActivityTagDetailModel, ActivityTagListEntityMapper>, IActivityTagFacade
{
    public ActivityTagFacade(
        IUnitOfWorkFactory unitOfWorkFactory,
        IActivityTagModelMapper activityTagModelMapper)
        : base(unitOfWorkFactory, activityTagModelMapper)
    {
    }
}
