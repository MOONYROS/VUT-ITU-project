namespace project.BL.Models;

public record ActivityTagDetailModel : ModelBase
{
    public required Guid ActivityId { get; set; }
    public required Guid TagId { get; set; }
    
    public static ActivityTagDetailModel Empty => new()
    {
        Id = Guid.NewGuid(),
        ActivityId = Guid.Empty,
        TagId = Guid.Empty
    };
}