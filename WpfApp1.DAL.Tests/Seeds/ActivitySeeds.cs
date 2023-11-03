using WpfApp1.DAL.Entities;

namespace WpfApp1.DAL.Tests.Seeds;

public static class ActivitySeeds
{
    private static int _counter = 0;
    private static int ActivityCounter() => ++_counter; 
    public static ActivityEntity ActivitySeed() => new()
    {
        Id = Guid.NewGuid(),
        DateTimeFrom = default,
        DateTimeTo = default,
        Name = $"random activity number {ActivityCounter()}",
        Color = 0,
        Project = null,
        ProjectId = null,
        User = null,
        UserId = default
    };

}