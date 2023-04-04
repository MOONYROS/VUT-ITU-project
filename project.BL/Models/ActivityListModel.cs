using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.BL.Models;
public record ActivityListModel : ModelBase
{
    public required string Name { get; set; }
    public required DateTime DateTimeFrom { get; set; }
    public required DateTime DateTimeTo { get; set; }
    public required Color Color { get; set; }
    public ObservableCollection<TagListModel> Tags { get; set; } = new();
    public static ActivityListModel Empty => new()
    {
        // TODO: Change default color to match UI
        Id = Guid.NewGuid(),
        Name = string.Empty,
        DateTimeFrom = default,
        DateTimeTo = default,
        Color = Color.Black
    };
}