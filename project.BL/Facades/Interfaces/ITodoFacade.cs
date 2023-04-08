﻿using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Facades;
public interface ITodoFacade : IFacade<TodoEntity, TodoListModel, TodoDetailModel>
{
}
