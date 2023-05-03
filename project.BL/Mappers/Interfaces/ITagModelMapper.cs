using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers.Interfaces;

public interface ITagModelMapper
{
    TagDetailModel MapToDetailModel(ActivityTagListEntity entity);
    IEnumerable<TagDetailModel> MapToDetailModel(IEnumerable<ActivityTagListEntity> entities)
        => entities.Select(MapToDetailModel);
}