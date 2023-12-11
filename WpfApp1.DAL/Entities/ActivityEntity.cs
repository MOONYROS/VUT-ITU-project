namespace WpfApp1.DAL.Entities;

public record ActivityEntity : IEntityID
{
    public required Guid Id { get; set; }
    public required DateTime DateTimeFrom { get; set; }
    public required DateTime DateTimeTo { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required int Color { get; set; }
    public ICollection<ActivityTagListEntity> Tags { get; init; } = new List<ActivityTagListEntity>();
    public ICollection<UserActivityListEntity> Users { get; init; } = new List<UserActivityListEntity>();
}