using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers.Interfaces;

public interface ITodoModelMapper : IModelMapper<TodoEntity, TodoListModel, TodoDetailModel>
{
    // TodoListModel MapToListModel(TodoDetailModel detail);
    // TodoEntity MapToEntity(TodoDetailModel model, Guid todoId);
    // void MapToExistingDetailModel(TodoDetailModel detail, TodoListModel todo);
    // TodoEntity MapToEntity(TodoListModel model);
}