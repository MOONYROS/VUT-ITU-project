﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace project.BL.Models;
public record UserDetailModel : ModelBase
{
    public required string UserName { get; set; }
    public required string FullName { get; set; }
    public string? ImageUrl { get; set; }
    public ObservableCollection<ActivityListModel> Activities { get; init; } = new();
    public ObservableCollection<ProjectListModel> Projects { get; init; } = new();
    public ObservableCollection<TagListModel> Tags { get; init; } = new();
    public ObservableCollection<TodoListModel> Todos { get; init; } = new();

    public static UserDetailModel Empty => new()
    {
        FullName = string.Empty,
        UserName = string.Empty,
        Id = Guid.Empty
    };
}
