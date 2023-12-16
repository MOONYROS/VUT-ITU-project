namespace WpfApp1.DAL.Entities;

public record ActivityTagListEntity : IEntityId
{
    public required Guid Id { get; set; }
    public required Guid ActivityId { get; set; }
    public ActivityEntity? Activity { get; init; }
    public required Guid TagId { get; set; }
    public TagEntity? Tag { get; init; }
}