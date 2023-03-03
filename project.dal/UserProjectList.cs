namespace project.dal
{
    public record UserProjectList
    {
        public Project Project { get; set; }
        public User User { get; set; }
    }
}