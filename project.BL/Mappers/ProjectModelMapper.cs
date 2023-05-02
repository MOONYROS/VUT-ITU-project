using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers;

public class ProjectModelMapper : ModelMapperBase<ProjectEntity, ProjectListModel, ProjectDetailModel>
{
    public override ProjectListModel MapToListModel(ProjectEntity? entity)
    {
        throw new NotImplementedException();
    }

    public override ProjectEntity MapToEntity(ProjectDetailModel model)
    {
        throw new NotImplementedException();
    }

    public override IEnumerable<ProjectListModel> MapToListModel(IEnumerable<ProjectEntity> entities)
    {
        throw new NotImplementedException();
    }

    public override ProjectDetailModel MapToDetailModel(ProjectEntity entity)
    {
        throw new NotImplementedException();
    }
}