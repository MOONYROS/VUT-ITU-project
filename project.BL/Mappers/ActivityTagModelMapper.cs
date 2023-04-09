using project.BL.Models;
using project.DAL.Entities;
using project.BL.Mappers.Interfaces;

namespace project.BL.Mappers;

public class ActivityTagModelMapper : ModelMapperBase<ActivityTagListEntity, ActivityTagListModel, ActivityTagDetailModel>,
    IActivityTagListMapper
{
    public ActivityTagListEntity MapToEntity(ActivityDetailModel activity, TagDetailModel tag)
        => new()
        {
            Id = Guid.NewGuid(),
            ActivityId = activity.Id,
            TagId = tag.Id
        };

    public void AddTagToActivity_Entities(ActivityEntity activity, TagEntity tag,
        ActivityTagListEntity tagInActivity)
    {
        activity.Tags.Add(tagInActivity);
        tag.Activities.Add(tagInActivity);
    }

    public void AddTagToActivity_Models(TagDetailModel tag, ActivityDetailModel activity)
    {
        activity.Tags.Add(tag);
    }

    public override ActivityTagListModel MapToListModel(ActivityTagListEntity? entity)
    {
        throw new NotSupportedException();
    }

    public override ActivityTagDetailModel MapToDetailModel(ActivityTagListEntity entity)
    {
        throw new NotSupportedException();
    }

    public override ActivityTagListEntity MapToEntity(ActivityTagDetailModel model)
    {
        throw new NotSupportedException();
    }
}
