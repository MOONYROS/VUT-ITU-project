using project.BL.Models;
using project.DAL.Entities;
using project.DAL.Mappers;

namespace project.BL.Facades.Interfaces;
public interface IUserProjectFacade : IFacade<UserProjectListEntity, UserProjectListModel, UserProjectDetailModel>
{
}
