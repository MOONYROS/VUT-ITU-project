using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;
using project.DAL.Mappers;
using project.DAL.UnitOfWork;

namespace project.BL.Facades;

public class TodoFacade :
    FacadeBase<TodoEntity, TodoListModel, TodoDetailModel, TodoEntityMapper>, ITodoFacade
{
    public TodoFacade(
        IUnitOfWorkFactory unitOfWorkFactory,
        ITodoModelMapper modelMapper)
        : base(unitOfWorkFactory, modelMapper)
    {
    }
}
