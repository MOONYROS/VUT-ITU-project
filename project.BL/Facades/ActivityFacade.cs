using project.BL.Facades.Interfaces;
using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;
using project.DAL.Mappers;
using project.DAL.UnitOfWork;

namespace project.BL.Facades;

public class ActivityFacade :
    FacadeBase<ActivityEntity, ActivityListModel, ActivityDetailModel, ActivityEntityMapper>, IActivityFacade
{
    public ActivityFacade(
        IUnitOfWorkFactory unitOfWorkFactory,
        IActivityModelMapper modelMapper)
        : base(unitOfWorkFactory, modelMapper)
    {
    }

    public override Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public override Task<ActivityDetailModel?> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public override Task<ActivityDetailModel> SaveAsync(ActivityDetailModel model)
    {
        throw new NotImplementedException();
    }

    public override Task<IEnumerable<ActivityListModel>> GetAsync()
    {
        throw new NotImplementedException();
    }

    public override Task<ActivityDetailModel> SaveAsync(ActivityDetailModel model, Guid id)
    {
        throw new NotImplementedException();
    }
}