namespace project.BL.Models;

public record UserProjectListModel : ModelBase
{
    public required Guid UserId { get; set; }
    public required Guid ProjectId { get; set; }

    public static UserProjectListModel Empty => new()
    {
        Id = Guid.NewGuid(),
        UserId = Guid.Empty,
        ProjectId = Guid.Empty
    };
}