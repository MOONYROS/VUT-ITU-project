using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;
using System.Diagnostics;
using System.Drawing;

namespace project.BL.Mappers;

public class UserModelMapper : ModelMapperBase<UserEntity, UserListModel, UserDetailModel>
{
    private readonly IActivityModelMapper _activityModelMapper;

    public UserModelMapper(IActivityModelMapper activityModelMapper)
    {
        _activityModelMapper = activityModelMapper;
    }

    public override UserListModel MapToListModel(UserEntity? entity)
    => entity is null ?
        UserListModel.Empty :
        new UserListModel
        {
            Id = entity.Id,
            UserName = entity.UserName,
            ImageUrl = entity.ImageUrl
        };

    public override UserEntity MapToEntity(UserDetailModel model)
    => new()
    {
        Id = model.Id,
        FullName = model.FullName,
        UserName = model.UserName,
        ImageUrl = model.ImageUrl
    };

    public override UserDetailModel MapToDetailModel(UserEntity entity)
    => entity is null ?
        UserDetailModel.Empty :
        new UserDetailModel
        {
            Id = entity.Id,
            FullName = entity.FullName,
            UserName = entity.UserName,
            ImageUrl = entity.ImageUrl,
            Activities = _activityModelMapper.MapToListModel(entity.Activities).ToObservableCollection()
        };
}