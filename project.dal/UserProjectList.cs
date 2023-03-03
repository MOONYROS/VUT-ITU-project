namespace project.dal
{
    public record UserProjectList : IEntityID
    {
        public Guid Id { get; set; }
        public Project Project { get; set; }
        public User User { get; set; }
    }
}