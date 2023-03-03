using System.Drawing;

namespace project.dal
{
    public record Todo : IEntityID
    {
        public Guid Id { get; set; }
        public string  Name { get; set; }
        public DateOnly Date { get; set; }
        public bool Finished { get; set; }
        public Color Color { get; set; }
        public User User { get; set; }
        //test koment
    }
}