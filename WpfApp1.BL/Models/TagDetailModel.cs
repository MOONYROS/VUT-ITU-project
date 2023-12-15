using System.Drawing;

namespace WpfApp1.BL.Models;

public record TagDetailModel : ModelBase
{
    public required string Name { get; set; }
    public required Color Color { get; set; }
        
    public static TagDetailModel Empty => new()
    {
        Id = Guid.Empty,
        Name = string.Empty,
        Color = Color.Black
    };
}