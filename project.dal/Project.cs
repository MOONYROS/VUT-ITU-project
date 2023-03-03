namespace project.dal
{
    public record Project : IEntityID
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }//mozna string?
        public ICollection<UserProjectList> Users { get; set; }
        public ICollection<Activity> Activities { get; set; }
    }
}