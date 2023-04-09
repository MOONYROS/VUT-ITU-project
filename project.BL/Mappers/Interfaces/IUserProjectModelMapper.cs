using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers.Interfaces;

public interface IUserProjectModelMapper : IModelMapper<UserProjectListEntity, UserProjectListModel, UserProjectDetailModel>
{
}