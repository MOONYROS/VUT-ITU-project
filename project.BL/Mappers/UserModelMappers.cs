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
                Id = entity.Id,
                UserName = entity.UserName
            };

    public override UserDetailModel MapToDetailModel(UserEntity? entity)
        => entity is null
            ? UserDetailModel.Empty
            : new UserDetailModel
            {
                Id = entity.Id,
                FullName = entity.FullName,
                UserName = entity.UserName
            };

    public override UserEntity MapToEntity(UserDetailModel model)
        => new()
        {
            Id = model.Id,
            FullName = model.FullName,
            UserName = model.UserName,
            ImageUrl = model.ImageUrl
        };

    public override UserEntity MapToEntity(UserDetailModel model, Guid guid)
    {
        throw new NotSupportedException();
    }
}