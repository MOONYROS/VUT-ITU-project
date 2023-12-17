using Microsoft.EntityFrameworkCore;
using WpfApp1.BL.Facades.Interfaces;
using WpfApp1.BL.Mappers.Interfaces;
using WpfApp1.BL.Models;
using WpfApp1.DAL.Entities;
using WpfApp1.DAL.Mappers;
using WpfApp1.DAL.UnitOfWork;

namespace WpfApp1.BL.Facades;

public class TagFacade :
    FacadeBaseDetailOnly<TagEntity, TagDetailModel, TagEntityMapper>, ITagFacade
{
    private readonly ITagModelMapper _tagModelMapper;
    public TagFacade(
        IUnitOfWorkFactory unitOfWorkFactory,
        ITagModelMapper modelMapper)
        : base(unitOfWorkFactory, modelMapper)
    {
        _tagModelMapper = modelMapper;
    }

    public async Task<IEnumerable<TagDetailModel>> GetAsyncUser(Guid userId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<TagEntity> entities = await uow
            .GetRepository<TagEntity, TagEntityMapper>()
            .Get()
            .Where(i => i.UserId == userId)
            .ToListAsync();

        return _tagModelMapper.MapToDetailModel(entities);
    }

    public async Task<IEnumerable<TagDetailModel>> GetAsyncActivity(Guid activityId)
    {
	    await using IUnitOfWork uow = UnitOfWorkFactory.Create();
	    List<TagEntity> entities = await uow
		    .GetRepository<TagEntity, TagEntityMapper>()
		    .Get()
		    .Include($"{nameof(TagEntity.Activities)}.{nameof(ActivityTagListEntity.Activity)}")
		    .Where(tag => tag.Activities.Any(activity => activity.ActivityId == activityId))
		    .ToListAsync();

	    return _tagModelMapper.MapToDetailModel(entities);
    }
}