namespace project.DAL.Entities
{
    public record UserEntity : IEntityID
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<UserProjectListEntity> Projects { get; set; }
        public ICollection<ActivityEntity> Activities { get; set; }
        public ICollection<TodoEntity>? Todos { get; set; }
    }
}