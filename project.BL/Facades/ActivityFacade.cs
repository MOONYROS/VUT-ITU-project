using Microsoft.EntityFrameworkCore;
using project.BL.Facades.Interfaces;
using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;
using project.DAL.Mappers;
using project.DAL.Repositories;
using project.DAL.UnitOfWork;

namespace project.BL.Facades;

public class ActivityFacade :
    FacadeBase<ActivityEntity, ActivityListModel, ActivityDetailModel, ActivityEntityMapper>, IActivityFacade
{
    protected readonly IActivityModelMapper _ModelMapper;
    public ActivityFacade(
        IUnitOfWorkFactory unitOfWorkFactory,
        IActivityModelMapper modelMapper)
        : base(unitOfWorkFactory, modelMapper)
    {
        _ModelMapper = modelMapper;
    }

    public override async Task DeleteAsync(Guid id)
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

        // query = query.Include($"{nameof(UserEntity.Activities)}.{nameof(ActivityEntity.Project)}");
        query = query.Include($"{nameof(ActivityEntity.Project)}");
        query = query.Include($"{nameof(ActivityEntity.Tags)}.{nameof(ActivityTagListEntity.Tag)}");


        ActivityEntity? entity = await query.SingleOrDefaultAsync(e => e.Id == id);

        return entity is null
            ? null
            : _ModelMapper.MapToDetailModel(entity);
    }

    public override Task<ActivityDetailModel> SaveAsync(ActivityDetailModel model)
    {
        throw new NotSupportedException();
    }

    public override async Task<IEnumerable<ActivityListModel>> GetAsync()
    {
        throw new NotImplementedException();//yet
    }

    public override async Task<ActivityDetailModel> SaveAsync(ActivityDetailModel model, Guid id)
    {
        ActivityDetailModel result;

        GuardCollectionsAreNotSet(model);

        ActivityEntity entity = _ModelMapper.MapToEntity(model, id, id);

        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<ActivityEntity> repository = uow.GetRepository<ActivityEntity, ActivityEntityMapper>();

        if (await repository.ExistsAsync(entity)) 
        {
            ActivityEntity updatedEntity = await repository.UpdateAsync(entity);
            result = _ModelMapper.MapToDetailModel(updatedEntity);
        }
        else
        {
            entity.Id = Guid.NewGuid();
            ActivityEntity insertedEntity = await repository.InsertAsync(entity);
            result = _ModelMapper.MapToDetailModel(entity);
        }

        await uow.CommitAsync();

        return result;
    }
}