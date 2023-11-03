using WpfApp1.DAL.Entities;

namespace WpfApp1.DAL.Tests.Seeds;

public static class TagSeeds
{
    private static int _counter = 0;
    private static int TagCounter() => ++_counter;

    public static TagEntity TagSeed() => new()
    {
        Id = Guid.NewGuid(),
        Name = $"Tag number {TagCounter()}",
        Color = 0,
        UserId = Guid.Empty
    };
}