using project.BL.Models;
using project.DAL.Entities;
using project.BL.Mappers.Interfaces;

namespace project.BL.Mappers;

public class ActivityTagListMapper : ModelMapperBase<ActivityTagListEntity, ActivityDetailModel, TagDetailModel>,
    IActivityTagListMapper
{
    public ActivityTagListEntity MapToEntity(ActivityDetailModel activity, TagDetailModel tag)
        => new()
        {
            Id = Guid.NewGuid(),
            ActivityId = activity.Id,
            TagId = tag.Id
        };

    public void MapActivityTagListToDetailModel(TagDetailModel tag, ActivityDetailModel activity)
    {
        activity.Tags.Add(tag);
    }
    
    public override ActivityDetailModel MapToListModel(ActivityTagListEntity? entity)
    {
        throw new NotSupportedException();
    }

    public override TagDetailModel MapToDetailModel(ActivityTagListEntity entity)
    {
        throw new NotSupportedException();
    }

    public override ActivityTagListEntity MapToEntity(TagDetailModel model)
    {
        throw new NotSupportedException();
    }
}
