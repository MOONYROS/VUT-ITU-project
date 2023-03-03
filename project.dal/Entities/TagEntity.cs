using System.Drawing;

namespace project.DAL.Entities
{
    public record TagEntity : IEntityID
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        //public Color Color { get; set; }
        public ICollection<ActivityTagListEntity>? Activities { get; set; } // Odstranit "?" (je tu kvuli testu)
    }
}