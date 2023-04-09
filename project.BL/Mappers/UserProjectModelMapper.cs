﻿using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers;

public class UserProjectModelMapper : ModelMapperBase<UserProjectListEntity, UserProjectListModel, UserProjectDetailModel>,
    IUserProjectModelMapper
{
    public override UserProjectListModel MapToListModel(UserProjectListEntity? entity)
        => entity is null
            ? UserProjectListModel.Empty
            : new UserProjectListModel
            {
                Id = entity.Id,
                UserId = entity.UserId,
                ProjectId = entity.ProjectId,
            };

    public override UserProjectDetailModel MapToDetailModel(UserProjectListEntity entity)
        => new()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            ProjectId = entity.ProjectId,
        };

    public override UserProjectListEntity MapToEntity(UserProjectDetailModel model)
        => new()
        {
            Id = model.Id,
            ProjectId = model.ProjectId,
            UserId = model.UserId,
        };

    public override UserProjectListEntity MapToEntity(UserProjectDetailModel model, Guid guid)
    {
        throw new NotSupportedException();
    }
}