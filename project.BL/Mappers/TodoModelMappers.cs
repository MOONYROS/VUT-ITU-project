using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers;

public class TodoModelMapper : ModelMapperBase<TodoEntity, TodoListModel, TodoDetailModel>,
    ITodoModelMapper
{
    public override TodoListModel MapToListModel(TodoEntity? entity)
        => entity is null
            ? TodoListModel.Empty
            : new TodoListModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Date = entity.Date,
                Finished = entity.Finished
            };

    public override TodoDetailModel MapToDetailModel(TodoEntity? entity)
        => entity is null
            ? TodoDetailModel.Empty
            : new TodoDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Date = entity.Date,
                Finished = entity.Finished
            };
    
    public TodoEntity MapToEntity(TodoDetailModel model, UserEntity user)
        => new()
        {
            Id = model.Id,
            Name = model.Name,
            Date = model.Date,
            Finished = model.Finished,
            User = user,
            UserId = user.Id, 
        };
    
    public override TodoEntity MapToEntity(TodoDetailModel model)
    {
        throw new NotSupportedException();
    }
}