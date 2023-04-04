using System.Drawing;

namespace project.BL.Models;
public record ProjectListModel : ModelBase
{
    public required string Name { get; set; }
    public Color Color { get; set; }

    public static ProjectListModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Name = string.Empty
    };

}
