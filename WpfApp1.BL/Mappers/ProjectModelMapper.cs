using System.Drawing;
using WpfApp1.BL.Mappers.Interfaces;
using WpfApp1.BL.Models;
using WpfApp1.DAL.Entities;
using WpfApp1.BL;
using System.Diagnostics;

namespace WpfApp1.BL.Mappers;

public class ProjectModelMapper : ModelMapperBase<ProjectEntity, ProjectListModel, ProjectDetailModel>,
    IProjectModelMapper
{
    public override ProjectListModel MapToListModel(ProjectEntity? entity)
        => entity is null ?
        ProjectListModel.Empty :
        new ProjectListModel
        {
            Name = entity.Name,
            Id = entity.Id
        };

    public override ProjectEntity MapToEntity(ProjectDetailModel project)
        => new()
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description
        };

    public override ProjectDetailModel MapToDetailModel(ProjectEntity? entity)
    {
        var userMapper = new UserModelMapper();
        return entity is null
            ? ProjectDetailModel.Empty
            : new ProjectDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Users = userMapper.MapToListModel(entity.Users).ToObservableCollection()
            };
    }
}