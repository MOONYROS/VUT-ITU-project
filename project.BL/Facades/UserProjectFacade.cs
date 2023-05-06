using project.BL.Facades.Interfaces;
using project.DAL.Entities;

namespace project.BL.Facades;

public class UserProjectFacade : IUserProjectFacade
{
    public Task SaveAsync(Guid userId, Guid projectId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid userId, Guid projectId)
    {
        throw new NotImplementedException();
    }
}