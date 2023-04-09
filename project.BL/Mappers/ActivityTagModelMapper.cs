using project.BL.Models;
using project.DAL.Entities;
using project.BL.Mappers.Interfaces;

namespace project.BL.Mappers;

public class ActivityTagModelMapper : ModelMapperBase<ActivityTagListEntity, ActivityTagListModel, ActivityTagDetailModel>,
    IActivityTagModelMapper
{
    public override ActivityTagListModel MapToListModel(ActivityTagListEntity? entity)
        => entity is null
            ? ActivityTagListModel.Empty
            : new ActivityTagListModel
            {
                Id = entity.Id,
                ActivityId = entity.ActivityId,
                TagId = entity.TagId
            };

    public override ActivityTagDetailModel MapToDetailModel(ActivityTagListEntity entity) => new()
    {
        Id = entity.Id,
        ActivityId = entity.ActivityId,
        TagId = entity.TagId
    };

    public override ActivityTagListEntity MapToEntity(ActivityTagDetailModel model) => new()
    {
        Id = model.Id,
        ActivityId = model.ActivityId,
        TagId = model.TagId,
    };

    public override ActivityTagListEntity MapToEntity(ActivityTagDetailModel model, Guid guid)
    {
        throw new NotSupportedException();
    }
}
