namespace project.BL.Models;

public record UserListModel : ModelBase
{
    public required string UserName { get; set; }
    public string? ImageUrl { get; set; }
    public static UserListModel Empty => new()
    {
        Id = Guid.NewGuid(),
        UserName = string.Empty
    };
}