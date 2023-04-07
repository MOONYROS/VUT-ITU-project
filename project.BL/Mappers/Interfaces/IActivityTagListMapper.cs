﻿using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers.Interfaces;

public interface IActivityTagListMapper : IModelMapper<ActivityTagListEntity, ActivityDetailModel, TagDetailModel>
{
}