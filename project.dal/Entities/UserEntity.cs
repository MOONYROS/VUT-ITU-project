namespace project.DAL.Entities
{
    public record UserEntity : IEntityID
    {
        public required Guid Id { get; set; }
        public required string FullName { get; set; }
        public required string UserName { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<UserProjectListEntity> Projects { get; set; }
        public ICollection<ActivityEntity> Activities { get; set; }
        public ICollection<TodoEntity> Todos { get; set; }
    }
}