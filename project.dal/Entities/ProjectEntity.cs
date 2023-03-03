namespace project.DAL.Entities
{
    public record ProjectEntity : IEntityID
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }//mozna string?
        public ICollection<UserProjectListEntity> Users { get; set; }
        public ICollection<ActivityEntity> Activities { get; set; }
    }
}