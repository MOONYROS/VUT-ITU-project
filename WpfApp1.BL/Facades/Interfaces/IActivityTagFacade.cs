using WpfApp1.DAL.Entities;

namespace WpfApp1.BL.Facades.Interfaces;

public interface IActivityTagFacade
{
    Task SaveAsync(Guid activityId, Guid tagId);
    Task DeleteAsync(Guid activityId, Guid tagId);

}