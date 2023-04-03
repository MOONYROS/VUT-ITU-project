namespace project.DAL.Entities
{
    public record ActivityTagListEntity : IEntityID
    {
        public Guid Id { get; set; }
        public required Guid ActivityEntityId { get; set; }
        public ActivityEntity Activity { get; set; }
        public required Guid TagEntityId { get; set; }
        public TagEntity Tag { get; set; }
    }
}