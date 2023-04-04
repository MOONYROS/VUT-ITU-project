 
using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers;

public class ProjectModelMappers : ModelMapperBase<ProjectEntity, ProjectListModel, ProjectDetailModel>,
    IProjectModelMapper
{
    public override ProjectListModel MapToListModel(ProjectEntity? entity)
        => entity is null
            ? ProjectListModel.Empty
            : new ProjectListModel()
            {
                Name = string.Empty
            };

    public override ProjectDetailModel MapToDetailModel(ProjectEntity? entity)
        => entity is null
            ? ProjectDetailModel.Empty
            : new ProjectDetailModel
            {
                Name = string.Empty,
                Description = string.Empty
            };

    public override ProjectEntity MapToEntity(ProjectDetailModel model)
        => new()
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            Description = string.Empty
        };
}