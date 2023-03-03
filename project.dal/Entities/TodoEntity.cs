using System.Drawing;

namespace project.DAL.Entities
{
    public record TodoEntity : IEntityID
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateOnly Date { get; set; }
        public bool Finished { get; set; }
        //public Color Color { get; set; }
        public UserEntity User { get; set; }
        //test koment
    }
}