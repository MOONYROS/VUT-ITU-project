using System.Drawing;

namespace project.BL.Models;
public record TagListModel : ModelBase
{
    public required string Name { get; set; }
    public required Color Color { get; set; }
    
    public static TagListModel Empty => new()
    {
        // Not necessary to implement TagDetailModel (same attributes will be listed)
        // TODO: Match default color with UI
        Id = Guid.NewGuid(),
        Name = string.Empty,
        Color = Color.Black
    };
}