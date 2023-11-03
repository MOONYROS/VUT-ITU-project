using WpfApp1.DAL.Entities;

namespace WpfApp1.DAL.Tests.Seeds;

public static class TodoSeeds
{
    private static int _counter = 0;
    private static int TodoCounter() => ++_counter;

    public static TodoEntity TodoSeed() => new()
    {
        Id = Guid.NewGuid(),
        Name = $"Todo number {TodoCounter()}",
        Date = default,
        Finished = false,
        UserId = default
    };

}