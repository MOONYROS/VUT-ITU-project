namespace project.DAL.Entities
{
    public record UserProjectListEntity : IEntityID
    {
        public Guid Id { get; set; }
        public ProjectEntity Project { get; set; }
        public UserEntity User { get; set; }
    }
}