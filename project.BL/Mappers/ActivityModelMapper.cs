using System.Drawing;
using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers;

public class ActivityModelMapper : ModelMapperBase<ActivityEntity, ActivityListModel, ActivityDetailModel>,
    IActivityModelMapper
{
    private readonly ProjectModelMapper _projectMapper;
    private readonly TagModelMapper _tagModelMapper;

    public ActivityModelMapper(ProjectModelMapper projectMapper, TagModelMapper tagModelMapper)
    {
        _projectMapper = projectMapper;
        _tagModelMapper = tagModelMapper;
    }

    public override ActivityListModel MapToListModel(ActivityEntity? entity)
        => entity is null ?
        ActivityListModel.Empty :
        new ActivityListModel
        {
            Name = entity.Name,
            DateTimeFrom = entity.DateTimeFrom,
            DateTimeTo = entity.DateTimeTo,
            Color = Color.FromArgb(entity.Color),
            Project = _projectMapper.MapToListModel(entity.Project),
            // Tady potrebujem tag entities, ne activityTag
            Tags = _tagModelMapper.MapToDetailModel(entity.Tags) 
        };
        

    public override ActivityEntity MapToEntity(ActivityDetailModel model)
    {
        throw new NotSupportedException();
    }

    public override IEnumerable<ActivityListModel> MapToListModel(IEnumerable<ActivityEntity> entities)
    {
        throw new NotImplementedException();
    }

    public override ActivityDetailModel MapToDetailModel(ActivityEntity? entity)
        => entity is null ?
        ActivityDetailModel.Empty : 
        new ActivityDetailModel
        {
            Name = entity.Name,
            DateTimeFrom = entity.DateTimeFrom,
            DateTimeTo = entity.DateTimeTo,
            Color = Color.FromArgb(entity.Color),
            Description = entity.Description,
            UserId = entity.UserId,
            Project = _projectMapper.MapToListModel(entity.Project)
        };
        

    public ActivityEntity MapToEntity(ActivityDetailModel activity, Guid userGuid, Guid? projectGuid)
        => new()
        {
            Id = activity.Id,
            DateTimeFrom = activity.DateTimeFrom,
            DateTimeTo = activity.DateTimeTo,
            Name = activity.Name,
            Description = activity.Description,
            Color = activity.Color.ToArgb(),
            UserId = userGuid,
            ProjectId = projectGuid
        };
    public ActivityEntity MapToEntity(ActivityDetailModel activity, Guid? projectGuid)
        => new()
        {
            Id = activity.Id,
            DateTimeFrom = activity.DateTimeFrom,
            DateTimeTo = activity.DateTimeTo,
            Name = activity.Name,
            Description = activity.Description,
            Color = activity.Color.ToArgb(),
            UserId = activity.UserId,
            ProjectId = projectGuid
        };
}