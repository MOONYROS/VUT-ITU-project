namespace project.DAL.Entities
{
    public record ActivityTagListEntity : IEntityID
    {
        public Guid Id { get; set; }
        public required Guid ActivityId { get; set; }
        public ActivityEntity Activity { get; set; }
        public required Guid TagId { get; set; }
        public TagEntity Tag { get; set; }
    }
}