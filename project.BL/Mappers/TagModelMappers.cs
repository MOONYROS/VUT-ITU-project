using System.Drawing;
using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.Mappers;

public class TagModelMapper : ModelMapperBaseDetailOnly<TagEntity, TagDetailModel>,
    ITagModelMapper
{
    public override TagDetailModel MapToDetailModel(TagEntity entity)
    {
        throw new NotImplementedException();
    }

    public override TagEntity MapToEntity(TagDetailModel model)
    {
        throw new NotImplementedException();
    }

    public override TagEntity MapToEntity(TagDetailModel model, Guid userGuid)
    {
        throw new NotImplementedException();
    }
}