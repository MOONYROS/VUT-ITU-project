using System.Collections;
using Microsoft.IdentityModel.Tokens;
using WpfApp1.BL.Models;
using WpfApp1.DAL.Entities;

namespace WpfApp1.BL.Mappers.Interfaces;

public interface ITagModelMapper : IModelMapperDetailOnly<TagEntity, TagDetailModel>
{
    TagDetailModel MapToDetailModel(ActivityTagListEntity entity);
    IEnumerable<TagDetailModel> MapToDetailModel(IEnumerable<ActivityTagListEntity> entities);
}