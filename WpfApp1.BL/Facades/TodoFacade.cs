using Microsoft.EntityFrameworkCore;
using WpfApp1.BL.Facades.Interfaces;
using WpfApp1.BL.Mappers.Interfaces;
using WpfApp1.BL.Models;
using WpfApp1.DAL.Entities;
using WpfApp1.DAL.Mappers;
using WpfApp1.DAL.UnitOfWork;

namespace WpfApp1.BL.Facades;

public class TodoFacade :
    FacadeBaseDetailOnly<TodoEntity, TodoDetailModel, TodoEntityMapper>, ITodoFacade
{
    private readonly ITodoModelMapper _todoModelMapper;
    public TodoFacade(
        IUnitOfWorkFactory unitOfWorkFactory,
        ITodoModelMapper modelMapper)
        : base(unitOfWorkFactory, modelMapper)
    {
        _todoModelMapper = modelMapper;
    }

    public async Task<IEnumerable<TodoDetailModel>> GetAsyncUser(Guid userId, bool done)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<TodoEntity> entities = await uow
            .GetRepository<TodoEntity, TodoEntityMapper>()
            .Get()
            .Where(i => i.UserId == userId )
            .Where(i=> i.Finished == done)
            .ToListAsync();

        return _todoModelMapper.MapToDetailModel(entities);
    }
}