using project.BL.Facades.Interfaces;
using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;
using project.DAL.Mappers;
using project.DAL.UnitOfWork;

namespace project.BL.Facades;

public class TodoFacade :
    FacadeBaseDetailOnly<TodoEntity, TodoDetailModel, TodoEntityMapper>, ITodoFacade
{
    public TodoFacade(
        IUnitOfWorkFactory unitOfWorkFactory,
        ITodoModelMapper modelMapper)
        : base(unitOfWorkFactory, modelMapper)
    {
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<TodoDetailModel?> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<TodoDetailModel> SaveAsync(TodoDetailModel model)
    {
        throw new NotImplementedException();
    }
}