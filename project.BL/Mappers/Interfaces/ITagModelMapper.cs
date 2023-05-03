using System.Collections;
using Microsoft.IdentityModel.Tokens;
using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers.Interfaces;

public interface ITagModelMapper : IModelMapperDetailOnly<TagEntity, TagDetailModel>
{
    TagDetailModel MapToDetailModel(ActivityTagListEntity entity);

    IEnumerable<TagDetailModel> MapToDetailModel(IEnumerable<ActivityTagListEntity> entities)
    {
        var activityTagListEntities = entities.ToList();
        return activityTagListEntities.IsNullOrEmpty() ? 
            Enumerable.Empty<TagDetailModel>() : 
            activityTagListEntities.Select(MapToDetailModel);
    }
}