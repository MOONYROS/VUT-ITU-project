using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers.Interfaces;

public interface IUserModelMapper : IModelMapper<UserEntity, UserListModel, UserDetailModel>
{
    // UserListModel MapToListModel(UserDetailModel detail);
    // UserEntity MapToEntity(UserDetailModel model, Guid userId);
    // void MapToExistingDetailModel(UserDetailModel detail, UserListModel user);
    // UserEntity MapToEntity(UserListModel model);
}