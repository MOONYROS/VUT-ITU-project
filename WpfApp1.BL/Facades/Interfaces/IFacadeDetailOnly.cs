using WpfApp1.BL.Models;
using WpfApp1.DAL.Entities;

namespace WpfApp1.BL.Facades.Interfaces;

public interface IFacadeDetailOnly<TEntity, TDetailModel>
    where TEntity : class, IEntityId
    where TDetailModel : class, IModel
{
    Task DeleteAsync(Guid id);
    Task<TDetailModel?> GetAsync(Guid id);
    Task<TDetailModel> SaveAsync(TDetailModel model, Guid userId);
}