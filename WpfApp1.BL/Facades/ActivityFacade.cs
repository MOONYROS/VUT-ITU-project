using Microsoft.EntityFrameworkCore;
using WpfApp1.BL.Enums;
using WpfApp1.BL.Facades.Interfaces;
using WpfApp1.BL.Mappers.Interfaces;
using WpfApp1.BL.Models;
using WpfApp1.DAL.Entities;
using WpfApp1.DAL.Mappers;
using WpfApp1.DAL.Repositories;
using WpfApp1.DAL.UnitOfWork;
using System.Diagnostics;

namespace WpfApp1.BL.Facades;

public class ActivityFacade :
    FacadeBase<ActivityEntity, ActivityListModel, ActivityDetailModel, ActivityEntityMapper>, IActivityFacade
{
    private readonly IActivityModelMapper _modelMapper;
    public ActivityFacade(
        IUnitOfWorkFactory unitOfWorkFactory,
        IActivityModelMapper modelMapper)
        : base(unitOfWorkFactory, modelMapper)
    {
        _modelMapper = modelMapper;
    }

    public async Task DeleteAsync(Guid activityId, Guid userId)
    {
	    await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        
	    IRepository<UserActivityListEntity> repository = uow.GetRepository<UserActivityListEntity, UserActivityListEntityMapper>();

	    IQueryable<UserActivityListEntity> query = repository.Get();
	    
	    UserActivityListEntity bindingEntity = await query.SingleAsync(i => i.ActivityId == activityId && i.UserId == userId);

	    try
	    {
		    uow.GetRepository<UserActivityListEntity, UserActivityListEntityMapper>().Delete(bindingEntity.Id);
		    await uow.CommitAsync().ConfigureAwait(false);
	    }
	    catch (DbUpdateException e)
	    {
		    throw new InvalidOperationException("Entity deletion failed.", e);
	    }
	    
	    query = query.Where(i => i.ActivityId == activityId);

	    var remainingBindings = await query.ToListAsync();

	    if (!remainingBindings.Any())
	    {
		    await DeleteAsync(activityId);
	    }
    }
    
    public override async Task DeleteAsync(Guid id)//id == userid
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        try
        {
            uow.GetRepository<ActivityEntity, ActivityEntityMapper>().Delete(id);
            await uow.CommitAsync().ConfigureAwait(false);
        }
        catch (DbUpdateException e)
        {
            throw new InvalidOperationException("Entity deletion failed.", e);
        }
    }

    public override async Task<ActivityDetailModel?> GetAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<ActivityEntity> query = uow.GetRepository<ActivityEntity, ActivityEntityMapper>().Get();
        
        query = query.Include($"{nameof(ActivityEntity.Tags)}.{nameof(ActivityTagListEntity.Tag)}");
        query = query.Include($"{nameof(ActivityEntity.Users)}.{nameof(UserActivityListEntity.User)}");

        ActivityEntity? entity = await query.SingleOrDefaultAsync(e => e.Id == id);

        return entity is null
            ? null
            : _modelMapper.MapToDetailModel(entity);
    }


    public async Task<ActivityDetailModel> SaveAsync(ActivityDetailModel model, IEnumerable<Guid> userIds)
    {
        GuardCollectionsAreNotSet(model);
    
        ActivityEntity entity = _modelMapper.MapToEntity(model);

        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<ActivityEntity> repository = uow.GetRepository<ActivityEntity, ActivityEntityMapper>();
        
        entity.Id = Guid.NewGuid();
        ActivityEntity insertedEntity = await repository.InsertAsync(entity);
        ActivityDetailModel result = _modelMapper.MapToDetailModel(entity);

        await uow.CommitAsync();
        
        foreach (var userId in userIds)
        {
	        var bindingEntity = new UserActivityListEntity
	        {
		        Id = Guid.NewGuid(),
		        UserId = userId,
		        ActivityId = entity.Id
	        };
	        await uow.GetRepository<UserActivityListEntity, UserActivityListEntityMapper>().InsertAsync(bindingEntity);
        }
        
        await uow.CommitAsync();

        return result;
    }

    public async Task<IEnumerable<ActivityListModel>> GetAsyncUser(Guid userId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        var bindings = await uow.GetRepository<UserActivityListEntity, UserActivityListEntityMapper>()
	        .Get().Where(i => i.UserId == userId).ToListAsync();

        IEnumerable<Guid> activityIds = new List<Guid>();
        foreach (var binding in bindings)
        {
	        activityIds = activityIds.Append(binding.ActivityId);
        }
        
        IQueryable<ActivityEntity> query = uow.GetRepository<ActivityEntity, ActivityEntityMapper>().Get().Where(i => activityIds.Contains(i.Id));
        
        query = query.Include($"{nameof(ActivityEntity.Tags)}.{nameof(ActivityTagListEntity.Tag)}");

        List<ActivityEntity> entities = await query.ToListAsync();

        return _modelMapper.MapToListModel(entities);
    }

    public async Task<IEnumerable<ActivityListModel>> GetAsyncDateFilter(Guid userId, DateTime? from, DateTime? to)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        
        var bindings = await uow.GetRepository<UserActivityListEntity, UserActivityListEntityMapper>()
	        .Get().Where(i => i.UserId == userId).ToListAsync();

        IEnumerable<Guid> activityIds = new List<Guid>();
        foreach (var binding in bindings)
        {
	        activityIds = activityIds.Append(binding.ActivityId);
        }
        
        IQueryable<ActivityEntity> query = uow.GetRepository<ActivityEntity, ActivityEntityMapper>().Get().Where(i => activityIds.Contains(i.Id));

        if (from == null && to == null)
        {
            throw new NotSupportedException();
        }
        else if (from == null) 
        {
            query = query.Where(i => i.DateTimeTo <= to);
        }
        else if (to == null)
        {
            query = query.Where(i => i.DateTimeFrom >= from);
        }
        else
        {
            query = query.Where(i => i.DateTimeFrom >= from && i.DateTimeTo <= to);
        }
        
        query = query.Include($"{nameof(ActivityEntity.Tags)}.{nameof(ActivityTagListEntity.Tag)}");

        List<ActivityEntity> entities = await query.ToListAsync();

        return _modelMapper.MapToListModel(entities);
    }

    public async Task<IEnumerable<ActivityListModel>> GetAsyncTagFilter(Guid userId, Guid tagId)
    {
	    await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        
	    var userBindings = await uow.GetRepository<UserActivityListEntity, UserActivityListEntityMapper>()
		    .Get().Where(i => i.UserId == userId).ToListAsync();
	    
	    var tagBindings = await uow.GetRepository<ActivityTagListEntity, ActivityTagListEntityMapper>()
		    .Get().Where(i => i.TagId == tagId).ToListAsync();

	    IEnumerable<Guid> userActivityIds = new List<Guid>();
	    foreach (var binding in userBindings)
	    {
		    userActivityIds = userActivityIds.Append(binding.ActivityId);
	    }
	    
	    IEnumerable<Guid> tagActivityIds = new List<Guid>();
	    foreach (var binding in tagBindings)
	    {
		    tagActivityIds = tagActivityIds.Append(binding.ActivityId);
	    }

	    var activityIds = userActivityIds.Intersect(tagActivityIds);
	    
	    IQueryable<ActivityEntity> query = uow.GetRepository<ActivityEntity, ActivityEntityMapper>().Get().Where(i => activityIds.Contains(i.Id));
	    
	    query = query.Include($"{nameof(ActivityEntity.Tags)}.{nameof(ActivityTagListEntity.Tag)}");

	    List<ActivityEntity> entities = await query.ToListAsync();

	    return _modelMapper.MapToListModel(entities);
    }

    public override Task<IEnumerable<ActivityListModel>> GetAsync()
    {
        throw new NotSupportedException();
    }

    public override async Task<ActivityDetailModel> SaveAsync(ActivityDetailModel model)
    {
	    await using IUnitOfWork uow = UnitOfWorkFactory.Create();

	    var entity = _modelMapper.MapToEntity(model);

	    ActivityEntity updatedEntity = await uow.GetRepository<ActivityEntity, ActivityEntityMapper>().UpdateAsync(entity);

	    await uow.CommitAsync();
	    
	    return _modelMapper.MapToDetailModel(updatedEntity);
    }

}