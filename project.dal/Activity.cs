using System.Drawing;

namespace project.dal
{
    public class Activity
    {
        public DateOnly DateFrom { get; set; }

        public TimeOnly TimeFrom { get; set; }

        public DateOnly DateTo { get; set; }

        public TimeOnly TimeTo { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }//mozna string?
        public Color Color { get; set; }
    }
}