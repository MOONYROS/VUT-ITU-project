using System.Drawing;
using WpfApp1.BL.Mappers.Interfaces;
using WpfApp1.BL.Models;
using WpfApp1.DAL.Entities;
using WpfApp1.BL;

namespace WpfApp1.BL.Mappers;

public class ActivityModelMapper : ModelMapperBase<ActivityEntity, ActivityListModel, ActivityDetailModel>,
    IActivityModelMapper
{
    public override ActivityListModel MapToListModel(ActivityEntity? entity)
    {
        var tagMapper = new TagModelMapper();
            return entity is null
            ? ActivityListModel.Empty
            : new ActivityListModel
            {
                Id = entity.Id,
                Name = entity.Name,
                DateTimeFrom = entity.DateTimeFrom,
                DateTimeTo = entity.DateTimeTo,
                Color = Color.FromArgb(entity.Color),
                Tags = tagMapper.MapToDetailModel(entity.Tags).ToObservableCollection()
            };
    }

    public override ActivityEntity MapToEntity(ActivityDetailModel model)
	    => new()
	    {
		    Id = model.Id,
		    DateTimeFrom = model.DateTimeFrom,
		    DateTimeTo = model.DateTimeTo,
		    Name = model.Name,
		    Description = model.Description,
		    Color = model.Color.ToArgb(),
		    UserId = model.UserId
	    };
    
    public override ActivityDetailModel MapToDetailModel(ActivityEntity? entity)
    {
        var tagMapper = new TagModelMapper();
        return entity is null
            ? ActivityDetailModel.Empty
            : new ActivityDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                DateTimeFrom = entity.DateTimeFrom,
                DateTimeTo = entity.DateTimeTo,
                Color = Color.FromArgb(entity.Color),
                Description = entity.Description,
                UserId = entity.UserId,
                Tags = tagMapper.MapToDetailModel(entity.Tags).ToObservableCollection()
            };
    }

    public ActivityEntity MapToEntity(ActivityDetailModel activity, Guid userGuid)
        => new()
        {
            Id = activity.Id,
            DateTimeFrom = activity.DateTimeFrom,
            DateTimeTo = activity.DateTimeTo,
            Name = activity.Name,
            Description = activity.Description,
            Color = activity.Color.ToArgb(),
            UserId = userGuid
        };
}