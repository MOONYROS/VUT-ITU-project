namespace WpfApp1.DAL.Entities;

public record UserEntity : IEntityId
{
    public required Guid Id { get; set; }
    public required string FullName { get; set; }
    public required string UserName { get; set; }
    public string? ImageUrl { get; set; }
    public ICollection<UserActivityListEntity> Activities { get; init; } = new List<UserActivityListEntity>();
    public ICollection<TodoEntity> Todos { get; init; } = new List<TodoEntity>();
    public ICollection<TagEntity> Tags { get; init; } = new List<TagEntity>();
}