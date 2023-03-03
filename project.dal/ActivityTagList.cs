namespace project.dal
{
    public record ActivityTagList : IEntityID
    {
        public Guid Id { get; set; }
        public Activity Activity { get; set; }
        public Tag Tag { get; set; }
    }
}