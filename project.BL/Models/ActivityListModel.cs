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
        public required TimeOnly TimeFrom { get; set; }
        public required TimeOnly TimeTo { get; set; }
        public required DateOnly DateFrom { get; set; }
        public required DateOnly DateTo { get; set; }
        public required int Color { get; set; }
        public ObservableCollection<TagListModel> Tags { get; set; } = new();
        public static ActivityListModel Empty => new()
        {
            Name = string.Empty
        };
    }
}