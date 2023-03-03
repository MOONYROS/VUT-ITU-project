namespace project.DAL.Entities
{
    public record ActivityTagListEntity : IEntityID
    {
        public Guid Id { get; set; }
        public ActivityEntity Activity { get; set; }
        public TagEntity Tag { get; set; }
    }
}