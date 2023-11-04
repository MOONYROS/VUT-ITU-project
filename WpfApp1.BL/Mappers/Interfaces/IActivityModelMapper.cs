using WpfApp1.BL.Models;
using WpfApp1.DAL.Entities;

namespace WpfApp1.BL.Mappers.Interfaces;

public interface IActivityModelMapper : IModelMapper<ActivityEntity, ActivityListModel, ActivityDetailModel>
{
    ActivityEntity MapToEntity(ActivityDetailModel activity, Guid userGuid);
}