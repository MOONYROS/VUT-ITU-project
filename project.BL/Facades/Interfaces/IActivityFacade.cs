﻿using project.BL.Facades.Interfaces;
using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Facades;

public interface IActivityFacade : IFacade<ActivityEntity, ActivityListModel, ActivityDetailModel>
{
}