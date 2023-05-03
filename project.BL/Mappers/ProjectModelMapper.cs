using System.Drawing;
using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;
using project.BL;
using System.Diagnostics;

namespace project.BL.Mappers;

public class ProjectModelMapper : ModelMapperBase<ProjectEntity, ProjectListModel, ProjectDetailModel>
{
    private readonly IUserModelMapper _userModelMapper;
    public ProjectModelMapper(IUserModelMapper userModelMapper)
    {
        _userModelMapper = userModelMapper;
    }
    public override ProjectListModel MapToListModel(ProjectEntity? entity)
        => entity is null ?
        ProjectListModel.Empty :
        new ProjectListModel
        {
            Name = entity.Name
        };

    public override ProjectEntity MapToEntity(ProjectDetailModel project)
    => new()
    {
        Id = project.Id,
        Name = project.Name,
        Description = project.Description
    };

    public override ProjectDetailModel MapToDetailModel(ProjectEntity? entity)
        => entity is null ?
        ProjectDetailModel.Empty :
        new ProjectDetailModel
        {
            Name = entity.Name,
            Description = entity.Description,
            Users = _userModelMapper.MapToListModel(entity.Users).ToObservableCollection()
        };
}