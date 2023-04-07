using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers.Interfaces;

public interface IUserModelMapper : IModelMapper<UserEntity, UserListModel, UserDetailModel>
{
}