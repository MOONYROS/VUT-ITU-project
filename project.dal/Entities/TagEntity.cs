using System.Drawing;

namespace project.DAL.Entities
{
    public record TagEntity : IEntityID
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        /* TODO: Trida Color je moc slozity typ do databaze 
         * public Color Color { get; set; } */
        public ICollection<ActivityTagListEntity> Activities { get; set; } 
    }
}