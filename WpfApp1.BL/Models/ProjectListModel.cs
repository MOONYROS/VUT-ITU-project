namespace WpfApp1.BL.Models;

public record ProjectListModel : ModelBase
{
    public required string Name { get; set; }

    public static ProjectListModel Empty => new()
    {
        Id = Guid.Empty,
        Name = string.Empty
    };

}