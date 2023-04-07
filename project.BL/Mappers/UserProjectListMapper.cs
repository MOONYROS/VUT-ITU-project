using Microsoft.Identity.Client;
using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers;

public class UserProjectListMapper : ModelMapperBase<UserProjectListEntity, ProjectDetailModel, UserDetailModel>,
    IUserProjectListMapper
{
    public UserProjectListEntity MapToEntity(ProjectDetailModel project, UserDetailModel user) 
        => new()
        {
            Id = Guid.NewGuid(),
            ProjectId = project.Id,
            UserId = user.Id
        };

    public override ProjectDetailModel MapToListModel(UserProjectListEntity? entity)
    {
        throw new NotImplementedException();
    }

    ProjectDetailModel IModelMapper<UserProjectListEntity, UserDetailModel, ProjectDetailModel>.MapToDetailModel(UserProjectListEntity entity)
    {
        throw new NotImplementedException();
    }

    public UserProjectListEntity MapToEntity(ProjectDetailModel model)
    {
        throw new NotImplementedException();
    }

    UserDetailModel IModelMapper<UserProjectListEntity, UserDetailModel, ProjectDetailModel>.MapToListModel(UserProjectListEntity? entity)
    {
        throw new NotImplementedException();
    }

    public override UserDetailModel MapToDetailModel(UserProjectListEntity entity)
    {
        throw new NotImplementedException();
    }

    public override UserProjectListEntity MapToEntity(UserDetailModel model)
    {
        throw new NotImplementedException();
    }
}