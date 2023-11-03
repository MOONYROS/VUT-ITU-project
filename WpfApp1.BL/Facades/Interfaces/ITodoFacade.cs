using WpfApp1.BL.Models;
using WpfApp1.DAL.Entities;

namespace WpfApp1.BL.Facades.Interfaces;

public interface ITodoFacade : IFacadeDetailOnly<TodoEntity, TodoDetailModel>
{
    Task<IEnumerable<TodoDetailModel>> GetAsyncUser(Guid userId);
}