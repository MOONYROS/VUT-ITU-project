using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers;

public class UserModelMapper : ModelMapperBase<UserEntity, UserListModel, UserDetailModel>,
    IUserModelMapper
{
    public override UserListModel MapToListModel(UserEntity? entity)
        => entity is null
            ? UserListModel.Empty
            : new UserListModel
            {
                Id = Guid.NewGuid(),
                UserName = entity.UserName
            };

    public override UserDetailModel MapToDetailModel(UserEntity? entity)
        => entity is null
            ? UserDetailModel.Empty
            : new UserDetailModel
            {
                FullName = entity.FullName,
                UserName = entity.UserName,
                Id = Guid.NewGuid()
            };

    public override UserEntity MapToEntity(UserDetailModel model)
        => new()
        {
            Id = Guid.NewGuid(),
            FullName = model.FullName,
            UserName = model.UserName,
            ImageUrl = model.ImageUrl
        };
}