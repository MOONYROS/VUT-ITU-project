namespace WpfApp1.DAL.Entities;

public record UserActivityListEntity : IEntityID
{
	public required Guid Id { get; set; }
	public required Guid ActivityId { get; set; }
	public ActivityEntity? Activity { get; init; }
	public required Guid UserId { get; set; }
	public UserEntity? User { get; init; }
}