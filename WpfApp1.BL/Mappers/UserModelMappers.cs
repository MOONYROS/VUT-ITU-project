using WpfApp1.BL.Mappers.Interfaces;
using WpfApp1.BL.Models;
using WpfApp1.DAL.Entities;
using System.Diagnostics;
using System.Drawing;
using Microsoft.IdentityModel.Tokens;

namespace WpfApp1.BL.Mappers;

public class UserModelMapper : ModelMapperBase<UserEntity, UserListModel, UserDetailModel>,
    IUserModelMapper
{
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

    public override UserDetailModel MapToDetailModel(UserEntity? entity)
    {
        var activityMapper = new ActivityModelMapper();
        return entity is null
            ? UserDetailModel.Empty
            : new UserDetailModel
            {
                Id = entity.Id,
                FullName = entity.FullName,
                UserName = entity.UserName,
                ImageUrl = entity.ImageUrl,
                Activities = activityMapper.MapToListModel(entity.Activities).ToObservableCollection()
            };
    }
    
}