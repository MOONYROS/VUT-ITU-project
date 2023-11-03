using WpfApp1.DAL.Entities;

namespace WpfApp1.BL.Facades.Interfaces;

public interface IUserProjectFacade
{
    Task SaveAsync(Guid userId, Guid projectId);
    Task DeleteAsync(Guid userId, Guid projectId);
}