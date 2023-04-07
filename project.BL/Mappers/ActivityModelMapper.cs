using System.Drawing;
using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers;

public class ActivityModelMapper : ModelMapperBase<ActivityEntity, ActivityListModel, ActivityDetailModel>,
    IActivityModelMapper
{
    public override ActivityListModel MapToListModel(ActivityEntity? entity)
        => entity is null
            ? ActivityListModel.Empty
            : new ActivityListModel
            {
                Id = entity.Id,
                Name = entity.Name,
                DateTimeFrom = entity.DateTimeFrom,
                DateTimeTo = entity.DateTimeTo,
                Color = Color.FromArgb(entity.Color)
            };

    public override ActivityDetailModel MapToDetailModel(ActivityEntity? entity)
        => entity is null
            ? ActivityDetailModel.Empty
            : new ActivityDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                DateTimeFrom = entity.DateTimeFrom,
                DateTimeTo = entity.DateTimeTo,
                Color = Color.FromArgb(entity.Color),
                Description = entity.Description
            };
    
    public ActivityEntity MapToEntity(ActivityDetailModel model, Guid userId, Guid? projectId) => new()
    {
        Id = model.Id,
        DateTimeFrom = model.DateTimeFrom,
        DateTimeTo = model.DateTimeTo,
        Name = model.Name,
        Color = model.Color.ToArgb(),
        Description = model.Description,
        Project = null,
        ProjectId = projectId,
        User = null,
        UserId = userId
    };
    
    public override ActivityEntity MapToEntity(ActivityDetailModel model)
    {
        throw new NotSupportedException();
    }
}