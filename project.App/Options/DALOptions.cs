namespace project.App.Options;

public record DALOptions
{
    public SqliteOptions? Sqlite { get; init; }
}

public record SqliteOptions
{
    public bool Enabled { get; init; }

    public string DatabaseName { get; init; } = null!;

    public bool RecreateDatabaseEachTime { get; init; } = false;
}
