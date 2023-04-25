﻿using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Facades.Interfaces;

public interface ITodoFacade : IFacade<TodoEntity, TodoListModel, TodoDetailModel>
{
}