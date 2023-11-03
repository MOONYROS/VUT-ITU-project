using WpfApp1.BL.Models;
using WpfApp1.DAL.Entities;

namespace WpfApp1.BL.Mappers.Interfaces;

public interface ITodoModelMapper : IModelMapperDetailOnly<TodoEntity, TodoDetailModel>
{
}