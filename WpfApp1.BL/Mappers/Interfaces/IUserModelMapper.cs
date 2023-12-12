using System.Collections;
using Microsoft.IdentityModel.Tokens;
using WpfApp1.BL.Models;
using WpfApp1.DAL.Entities;

namespace WpfApp1.BL.Mappers.Interfaces;
public interface IUserModelMapper : IModelMapper<UserEntity, UserListModel, UserDetailModel>
{
	UserListModel MapToListModel(UserActivityListEntity entity);
	IEnumerable<UserListModel> MapToListModel(IEnumerable<UserActivityListEntity> entities);
}