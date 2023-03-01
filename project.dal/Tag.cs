using System.Drawing;

namespace project.dal
{
    public class Tag
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public ICollection<ActivityTagList>? Activities { get; set; } // Odstranit "?" (je tu kvuli testu)
    }
}