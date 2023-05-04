using project.BL.Facades.Interfaces;
using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;
using project.DAL.Mappers;
using project.DAL.UnitOfWork;

namespace project.BL.Facades;

public class TagFacade :
    FacadeBaseDetailOnly<TagEntity, TagDetailModel, TagEntityMapper>, ITagFacade
{
    public TagFacade(
        IUnitOfWorkFactory unitOfWorkFactory,
        ITagModelMapper modelMapper)
        : base(unitOfWorkFactory, modelMapper)
    {
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<TagDetailModel?> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<TagDetailModel> SaveAsync(TagDetailModel model)
    {
        throw new NotImplementedException();
    }
}