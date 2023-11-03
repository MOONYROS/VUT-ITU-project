using WpfApp1.DAL.Entities;

namespace WpfApp1.DAL.Tests.Seeds;

public static class UserSeeds
{
    private static int _counter = 0;
    private static int UserCounter() => ++_counter;

    public static UserEntity UserSeed() => new()
    {
        Id = Guid.NewGuid(),
        FullName = "Random name",
        UserName = $"UserName number {UserCounter()}"
    };
}