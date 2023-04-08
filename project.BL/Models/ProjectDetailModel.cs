using System.Collections.ObjectModel;
using System.Drawing;

namespace project.BL.Models;

public record ProjectDetailModel : ModelBase
{
    public required string Name { get; set; }
    public Color Color { get; set; }
    public required string Description { get; set; }
    public ObservableCollection<UserDetailModel> Users { get; init; } = new();

    public static ProjectDetailModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Name = string.Empty,
        Description = string.Empty
    };
}