namespace project.BL.Models;

public record ActivityTagListModel : ModelBase
{
    public required Guid ActivityId { get; set; }
    public required Guid TagId { get; set; }
    
    public static ActivityTagListModel Empty => new()
    {
        Id = Guid.NewGuid(),
        ActivityId = Guid.Empty,
        TagId = Guid.Empty
    };
}