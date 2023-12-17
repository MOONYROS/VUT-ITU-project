using WpfApp1.BL.Models;
using WpfApp1.DAL.Entities;

namespace WpfApp1.BL.Facades.Interfaces;

public interface ITagFacade : IFacadeDetailOnly<TagEntity, TagDetailModel>
{
    Task<IEnumerable<TagDetailModel>> GetAsyncUser(Guid userId);
    Task<IEnumerable<TagDetailModel>> GetAsyncActivity(Guid activityId);
}