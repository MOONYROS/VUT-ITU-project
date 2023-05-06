using Microsoft.EntityFrameworkCore;
using project.BL.Facades.Interfaces;
using project.DAL.Entities;
using project.DAL.Mappers;
using project.DAL.Repositories;
using project.DAL.UnitOfWork;
using System.Threading.Tasks;

namespace project.BL.Facades;

public class ActivityTagFacade : IActivityTagFacade
{
    protected readonly IUnitOfWorkFactory UnitOfWorkFactory;
    public ActivityTagFacade(IUnitOfWorkFactory unitOfWorkFactory)
    {
        UnitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task SaveAsync(Guid activityId, Guid tagId)
    {
        ActivityTagListEntity bindingEntity = new()
        {
            Id = Guid.NewGuid(),
            TagId = tagId,
            ActivityId = activityId
        };

        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<ActivityTagListEntity> repository = uow.GetRepository<ActivityTagListEntity, ActivityTagListEntityMapper>();

        await repository.InsertAsync(bindingEntity);

        await uow.CommitAsync();
    }

    public async Task DeleteAsync(Guid activityId, Guid tagId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<ActivityTagListEntity> query = uow.GetRepository<ActivityTagListEntity, ActivityTagListEntityMapper>().Get();

        ActivityTagListEntity bindingEntity = await query.SingleAsync(i => i.ActivityId == activityId && i.TagId == tagId);

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