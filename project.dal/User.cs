namespace project.dal
{
    public record User
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<UserProjectList> Projects { get; set; }
        public ICollection<Activity> Activities { get; set; }
        public ICollection<Todo> Todos { get; set; }
    }
}