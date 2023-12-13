using System.Collections;
using Microsoft.EntityFrameworkCore;
using WpfApp1.BL.Facades.Interfaces;
using WpfApp1.BL.Mappers;
using WpfApp1.BL.Mappers.Interfaces;
using WpfApp1.BL.Models;
using WpfApp1.DAL.Entities;
using WpfApp1.DAL.Mappers;
using WpfApp1.DAL.Repositories;
using WpfApp1.DAL.UnitOfWork;

namespace WpfApp1.BL.Facades;

public class UserFacade :
    FacadeBase<UserEntity, UserListModel, UserDetailModel, UserEntityMapper>, IUserFacade
{
    private readonly IUserModelMapper _userModelMapper;
    public UserFacade(
        IUnitOfWorkFactory unitOfWorkFactory,
        IUserModelMapper modelMapper)
        : base(unitOfWorkFactory, modelMapper)
    {
        _userModelMapper = modelMapper;
    }

    public override async Task DeleteAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IEnumerable<UserActivityListEntity> bindingEntities = await uow.GetRepository<UserActivityListEntity, UserActivityListEntityMapper>()
	        .Get()
	        .Where(i => i.UserId == id)
	        .ToListAsync();

        foreach (var bindingEntity in  bindingEntities)
        {
	        var activityFacade = new ActivityFacade(UnitOfWorkFactory, new ActivityModelMapper());
	        await activityFacade.RemoveActivityFromUserAsync(bindingEntity.ActivityId, bindingEntity.UserId);
        }

        try
        {
            uow.GetRepository<UserEntity, UserEntityMapper>().Delete(id);
            await uow.CommitAsync().ConfigureAwait(false);
        }
        catch (DbUpdateException e)
        {
            throw new InvalidOperationException("Entity deletion failed.", e);
        }
    }

    public override async Task<UserDetailModel> SaveAsync(UserDetailModel model)
    {
        UserDetailModel result;

        GuardCollectionsAreNotSet(model);

        UserEntity entity = _userModelMapper.MapToEntity(model);

        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<UserEntity> repository = uow.GetRepository<UserEntity, UserEntityMapper>();

        if (await repository.ExistsAsync(entity)) 
        {
            UserEntity updatedEntity = await repository.UpdateAsync(entity);
            result = _userModelMapper.MapToDetailModel(updatedEntity);
        }
        else
        {
            entity.Id = Guid.NewGuid();
            UserEntity insertedEntity = await repository.InsertAsync(entity);
            result = _userModelMapper.MapToDetailModel(insertedEntity);
        }

        await uow.CommitAsync();

        return result;
    }

    public override async Task<UserDetailModel?> GetAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        UserEntity? entity = await uow.GetRepository<UserEntity, UserEntityMapper>()
	        .Get()
	        .SingleOrDefaultAsync(e => e.Id == id);

        return entity is null
            ? null
            : _userModelMapper.MapToDetailModel(entity);
    }

    public override async Task<IEnumerable<UserListModel>> GetAsync()
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        List<UserEntity> entities = await uow.GetRepository<UserEntity, UserEntityMapper>()
	        .Get()
	        .ToListAsync();
        return _userModelMapper.MapToListModel(entities);
    }
}