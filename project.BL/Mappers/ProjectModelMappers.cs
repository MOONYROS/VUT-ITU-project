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
                Id = entity.Id,
                Name = entity.Name,
            };

    public override ProjectDetailModel MapToDetailModel(ProjectEntity? entity)
        => entity is null
            ? ProjectDetailModel.Empty
            : new ProjectDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };

    public override ProjectEntity MapToEntity(ProjectDetailModel model)
        => new()
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description
        };
}