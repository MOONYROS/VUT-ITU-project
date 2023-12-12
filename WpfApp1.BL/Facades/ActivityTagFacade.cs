using Microsoft.EntityFrameworkCore;
using WpfApp1.BL.Facades.Interfaces;
using WpfApp1.DAL.Entities;
using WpfApp1.DAL.Mappers;
using WpfApp1.DAL.Repositories;
using WpfApp1.DAL.UnitOfWork;
using System.Threading.Tasks;

namespace WpfApp1.BL.Facades;

public class ActivityTagFacade : IActivityTagFacade
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    public ActivityTagFacade(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task SaveAsync(Guid activityId, Guid tagId)
    {
        ActivityTagListEntity bindingEntity = new()
        {
            Id = Guid.NewGuid(),
            TagId = tagId,
            ActivityId = activityId
        };

        await using IUnitOfWork uow = _unitOfWorkFactory.Create();
		await uow.GetRepository<ActivityTagListEntity, ActivityTagListEntityMapper>()
			.InsertAsync(bindingEntity);

        await uow.CommitAsync();
    }

    public async Task DeleteAsync(Guid activityId, Guid tagId)
    {
        await using IUnitOfWork uow = _unitOfWorkFactory.Create();

        ActivityTagListEntity bindingEntity = await uow.GetRepository<ActivityTagListEntity, ActivityTagListEntityMapper>()
	        .Get()
	        .SingleAsync(i => i.ActivityId == activityId && i.TagId == tagId);

        try
        {
            uow.GetRepository<ActivityTagListEntity, ActivityTagListEntityMapper>().Delete(bindingEntity.Id);
            await uow.CommitAsync().ConfigureAwait(false);
        }
        catch (DbUpdateException e)
        {
            throw new InvalidOperationException("Entity deletion failed.", e);
        }
    }
}