using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers.Interfaces;

public interface ITagModelMapper : IModelMapper<TagEntity, TagListModel, TagDetailModel>
{
    // TagListModel MapToListModel(TagDetailModel detail);
    // TagEntity MapToEntity(TagDetailModel model, Guid tagId);
    // void MapToExistingDetailModel(TagDetailModel detail, TagListModel tag);
    // TagEntity MapToEntity(TagListModel model);
}