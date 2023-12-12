using WpfApp1.BL.Enums;
using WpfApp1.BL.Models;
using WpfApp1.DAL.Entities;

namespace WpfApp1.BL.Facades.Interfaces;

public interface IActivityFacade : IFacade<ActivityEntity, ActivityListModel, ActivityDetailModel>
{
	Task DeleteAsync(Guid activityId, Guid userId);
    Task<ActivityDetailModel> SaveAsync(ActivityDetailModel model, IEnumerable<Guid> userIds);
    Task<IEnumerable<ActivityListModel>> GetAsyncUser(Guid userId);
    Task<IEnumerable<ActivityListModel>> GetAsyncDateFilter(Guid userId, DateTime? from, DateTime? to);
    Task<IEnumerable<ActivityListModel>> GetAsyncTagFilter(Guid userId, Guid tagId);
}