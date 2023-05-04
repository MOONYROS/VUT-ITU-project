using project.BL.Facades.Interfaces;
using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;
using project.DAL.Mappers;
using project.DAL.UnitOfWork;

namespace project.BL.Facades;

public class ProjectFacade :
    FacadeBase<ProjectEntity, ProjectListModel, ProjectDetailModel, ProjectEntityMapper>, IProjectFacade
{
    public ProjectFacade(
        IUnitOfWorkFactory unitOfWorkFactory,
        IProjectModelMapper modelMapper)
        : base(unitOfWorkFactory, modelMapper)
    {
    }

    public override Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public override Task<ProjectDetailModel?> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public override Task<ProjectDetailModel> SaveAsync(ProjectDetailModel model)
    {
        throw new NotImplementedException();
    }

    public override Task<IEnumerable<ProjectListModel>> GetAsync()
    {
        throw new NotImplementedException();
    }

    public override Task<ProjectDetailModel> SaveAsync(ProjectDetailModel model, Guid id)
    {
        throw new NotImplementedException();
    }
}