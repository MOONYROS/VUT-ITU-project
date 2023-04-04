namespace project.BL.Models
{
    public record TodoListModel : ModelBase
    {
        public required string Name { get; set; }
        public required DateOnly Date { get; set; }
        public required bool Finished { get; set; }

        public static TodoListModel Empty => new()
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            Date = default,
            Finished = false
        };

    }
}