using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.BL.Models
{
    public record ActivityListModel : ModelBase
    {
        public required string Name { get; set; }
        public required DateTime DateTimeFrom { get; set; }
        public required DateTime DateTimeTo { get; set; }
        public required int Color { get; set; }
        public ObservableCollection<TagListModel> Tags { get; set; } = new();
        public static ActivityListModel Empty => new()
        {
            Name = string.Empty
        };
    }
}