using project.BL.Facades.Interfaces;
using project.DAL.Entities;

namespace project.BL.Facades;

public class ActivityTagFacade : IActivityTagFacade
{
    public Task SaveAsync(Guid activityId, Guid tagId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid activityId, Guid tagId)
    {
        throw new NotImplementedException();
    }
}