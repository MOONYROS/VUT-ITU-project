using System.Drawing;
using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers;

public class TagModelMapper : ModelMapperBase<TagEntity, TagListModel, TagDetailModel>,
    ITagModelMapper
{
    public override TagListModel MapToListModel(TagEntity? entity)
        => entity is null
            ? TagListModel.Empty
            : new TagListModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Color = Color.FromArgb(entity.Color)
            };

    public override TagDetailModel MapToDetailModel(TagEntity? entity)
        => entity is null
            ? TagDetailModel.Empty
            : new TagDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Color = Color.FromArgb(entity.Color)
            };

    public override TagEntity MapToEntity(TagDetailModel model)
        => new()
        {
            Id = model.Id,
            Name = model.Name,
            Color = model.Color.ToArgb()
        };

    public override TagEntity MapToEntity(TagDetailModel model, Guid guid)
    {
        throw new NotSupportedException();
    }
}