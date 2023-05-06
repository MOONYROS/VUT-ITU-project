using project.BL.Enums;
using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Facades.Interfaces;

public interface IActivityFacade : IFacade<ActivityEntity, ActivityListModel, ActivityDetailModel>
{
    Task<ActivityDetailModel> SaveAsync(ActivityDetailModel model, Guid userId, Guid? projectId);
    Task<IEnumerable<ActivityListModel>> GetAsyncUser(Guid userId);
    Task<IEnumerable<ActivityListModel>> GetAsyncDate(Guid userId, DateTime from, DateTime to);
    Task<IEnumerable<ActivityListModel>> GetAsyncFilter(Guid userId, FilterBy interval);
}