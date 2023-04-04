using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.BL.Models
{
    public record UserListModel : ModelBase
    {
        public required string Name { get; set; }
        public string? ImageUrl { get; set; }
        public static UserListModel Empty => new()
        {
            Name = string.Empty,
            Id = Guid.NewGuid()
        };
    }
}
