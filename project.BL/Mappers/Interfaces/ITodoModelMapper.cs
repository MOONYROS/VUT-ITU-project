using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers.Interfaces;

public interface ITodoModelMapper : IModelMapper<TodoEntity, TodoListModel, TodoDetailModel>
{
    public TodoEntity MapToEntity(TodoDetailModel model, UserEntity user);
}