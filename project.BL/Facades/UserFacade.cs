using project.BL.Facades.Interfaces;
using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;
using project.DAL.Mappers;
using project.DAL.UnitOfWork;

namespace project.BL.Facades;

public class UserFacade :
    FacadeBase<UserEntity, UserListModel, UserDetailModel, UserEntityMapper>, IUserFacade
{
    public UserFacade(
        IUnitOfWorkFactory unitOfWorkFactory,
        IUserModelMapper modelMapper)
        : base(unitOfWorkFactory, modelMapper)
    {
    }
    protected override string IncludesNavigationPathDetail =>
        $"{nameof(UserEntity.Todos)}";

    public override Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public override Task<UserDetailModel?> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public override Task<UserDetailModel> SaveAsync(UserDetailModel model)
    {
        throw new NotImplementedException();
    }

    public override Task<IEnumerable<UserListModel>> GetAsync()
    {
        throw new NotImplementedException();
    }

    public override Task<UserDetailModel> SaveAsync(UserDetailModel model, Guid id)
    {
        throw new NotImplementedException();
    }
}