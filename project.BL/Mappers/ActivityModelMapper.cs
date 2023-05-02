﻿using System.Drawing;
using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers;

public class ActivityModelMapper : ModelMapperBase<ActivityEntity, ActivityListModel, ActivityDetailModel>,
    IActivityModelMapper
{
    private readonly ProjectModelMapper _projectMapper;
    public ActivityModelMapper(ProjectModelMapper projectMapper)
        => _projectMapper = projectMapper;
    public override ActivityListModel MapToListModel(ActivityEntity? entity)
    {
        throw new NotImplementedException();
    }

    public override ActivityEntity MapToEntity(ActivityDetailModel model)
    {
        throw new NotSupportedException();
    }

    public override IEnumerable<ActivityListModel> MapToListModel(IEnumerable<ActivityEntity> entities)
    {
        throw new NotImplementedException();
    }

    public override ActivityDetailModel MapToDetailModel(ActivityEntity entity)
        => new()
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