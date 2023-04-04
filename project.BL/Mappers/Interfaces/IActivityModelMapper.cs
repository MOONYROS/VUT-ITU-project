using System.Diagnostics;
using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers.Interfaces;

public interface IActivityModelMapper : IModelMapper<ActivityEntity, ActivityListModel, ActivityDetailModel>
{
    //ActivityListModel MapToListModel(ActivityDetailModel detail);
    // ActivityEntity MapToEntity(ActivityDetailModel model, Guid activityId);
    //void MapToExistingDetailModel(ActivityDetailModel detail, ProjectListModel activity);
    //ActivityEntity MapToEntity(ActivityListModel model, Guid activityId); 
}