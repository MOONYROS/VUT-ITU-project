using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers.Interfaces;

public interface IProjectModelMapper : IModelMapper<ProjectEntity, ProjectListModel, ProjectDetailModel>
{
    // ProjectListModel MapToListModel(ProjectDetailModel detail);
    // ProjectEntity MapToEntity(ProjectDetailModel model, Guid projectId);
    // void MapToExistingDetailModel(ProjectDetailModel detail, ProjectListModel project);
    // ProjectEntity MapToEntity(ProjectListModel model);
}