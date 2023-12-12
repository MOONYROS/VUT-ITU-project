using WpfApp1.BL.Models;
using WpfApp1.DAL.Entities;

namespace WpfApp1.BL.Facades.Interfaces;

public interface IActivityFacade : IFacade<ActivityEntity, ActivityListModel, ActivityDetailModel>
{
	Task RemoveActivityFromUserAsync(Guid activityId, Guid userId);
    Task<ActivityDetailModel> CreateActivityAsync(ActivityDetailModel model, IEnumerable<Guid> userIds);
    Task<IEnumerable<ActivityListModel>> GetUserActivitiesAsync(Guid userId);
    Task<IEnumerable<ActivityListModel>> GetActivitiesDateFilterAsync(Guid userId, DateTime? from, DateTime? to);
    Task<IEnumerable<ActivityListModel>> GetActivitiesTagFilterAsync(Guid userId, Guid tagId);
}