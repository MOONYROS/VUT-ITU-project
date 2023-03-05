    using System.Drawing;

namespace project.DAL.Entities
{
    public record ActivityEntity : IEntityID
    {
        public Guid Id { get; set; }
        public DateOnly DateFrom { get; set; }
        public TimeOnly TimeFrom { get; set; }
        public DateOnly DateTo { get; set; }
        public TimeOnly TimeTo { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        /* TODO: Trida Color je moc slozity typ do databaze 
         * public Color Color { get; set; } */
        public ICollection<ActivityTagListEntity> Tags { get; set; }
        public ProjectEntity? Project { get; set; }
        public UserEntity User { get; set; }
    }
}