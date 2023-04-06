using System.Drawing;

namespace project.DAL.Entities
{
    public record TagEntity : IEntityID
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required int Color { get; set; }
        public ICollection<ActivityTagListEntity> Activities { get; init; } = new List<ActivityTagListEntity>();
    }
}