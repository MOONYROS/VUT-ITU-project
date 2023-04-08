using project.BL.Mappers;
using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;
using project.DAL.Mappers;
using project.DAL.UnitOfWork;

namespace project.BL.Facades;

public class ActivityTagFacade  :
    FacadeBase<ActivityTagListEntity, ActivityListModel, TagDetailModel, ActivityTagListEntityMapper>, IActivityTagFacade
{
    private readonly IActivityTagListMapper _activityTagListMapper;
    public ActivityTagFacade(   
        IUnitOfWorkFactory unitOfWorkFactory,
        IActivityTagListMapper activityTagListMapper)
        : base(unitOfWorkFactory, activityTagListMapper) =>
        _activityTagListMapper = activityTagListMapper;

}
