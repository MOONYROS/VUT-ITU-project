using Microsoft.Identity.Client;
using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers;

public class UserProjectModelMapper : ModelMapperBase<UserProjectListEntity, UserProjectListModel, UserProjectDetailModel>,
    IUserProjectListMapper
{
    public UserProjectListEntity MapToEntity(ProjectDetailModel project, UserDetailModel user) 
        => new()
        {
            Id = Guid.NewGuid(),
            ProjectId = project.Id,
            UserId = user.Id
        };

    public void ConnectUserWithProjectModel(UserDetailModel user, ProjectDetailModel project)
    {
        project.Users.Add(user);
        user.Projects.Add(project);
    }
    
    public override ProjectDetailModel MapToListModel(UserProjectListEntity? entity)
    {
        throw new NotSupportedException();
    }

    ProjectDetailModel IModelMapper<UserProjectListEntity, UserDetailModel, ProjectDetailModel>.MapToDetailModel(UserProjectListEntity entity)
    {
        throw new NotSupportedException();
    }

    public UserProjectListEntity MapToEntity(ProjectDetailModel model)
    {
        throw new NotSupportedException();
    }

    UserDetailModel IModelMapper<UserProjectListEntity, UserDetailModel, ProjectDetailModel>.MapToListModel(UserProjectListEntity? entity)
    {
        throw new NotSupportedException();
    }

    public override UserDetailModel MapToDetailModel(UserProjectListEntity entity)
    {
        throw new NotSupportedException();
    }

    public override UserProjectListEntity MapToEntity(UserDetailModel model)
    {
        throw new NotSupportedException();
    }
}