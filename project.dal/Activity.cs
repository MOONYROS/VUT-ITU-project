using System.Drawing;

namespace project.dal
{
    public record Activity : IEntityID
    {
        public Guid Id { get; set; }
        public DateOnly DateFrom { get; set; }
        public TimeOnly TimeFrom { get; set; }
        public DateOnly DateTo { get; set; }
        public TimeOnly TimeTo { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Color Color { get; set; }
        public ICollection<ActivityTagList> Tags { get; set;}
        public Project? Project { get; set; }
        public User User { get; set; }
    }
}