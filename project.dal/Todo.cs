using System.Drawing;

namespace project.dal
{

    public class Todo
    {
        public string  Name { get; set; }
        public DateOnly Date { get; set; }
        public bool Finished { get; set; }
        public Color Color { get; set; }
    }
}