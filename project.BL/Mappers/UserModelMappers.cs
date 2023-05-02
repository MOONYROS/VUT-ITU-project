using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers;

public class UserModelMapper : ModelMapperBase<UserEntity, UserListModel, UserDetailModel>,
    IUserModelMapper
{
    public override UserListModel MapToListModel(UserEntity? entity)
    {
        throw new NotImplementedException();
    }

    public override UserEntity MapToEntity(UserDetailModel model)
    {
        throw new NotImplementedException();
    }

    public override IEnumerable<UserListModel> MapToListModel(IEnumerable<UserEntity> entities)
    {
        throw new NotImplementedException();
    }

    public override UserDetailModel MapToDetailModel(UserEntity entity)
    {
        throw new NotImplementedException();
    }
}