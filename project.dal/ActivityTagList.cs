namespace project.dal
{
    public record ActivityTagList
    {
        public Activity Activity { get; set; }
        public Tag Tag { get; set; }
    }
}