using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers;

public class TodoModelMapper : ModelMapperBaseDetailOnly<TodoEntity, TodoDetailModel>
{
    public override TodoDetailModel MapToDetailModel(TodoEntity entity)
    {
        throw new NotImplementedException();
    }

    public override TodoEntity MapToEntity(TodoDetailModel model)
    {
        throw new NotImplementedException();
    }

    public override TodoEntity MapToEntity(TodoDetailModel model, Guid userGuid)
    {
        throw new NotImplementedException();
    }
}