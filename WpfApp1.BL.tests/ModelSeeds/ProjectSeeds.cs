﻿using WpfApp1.BL.Models;

namespace WpfApp1.BL.tests.ModelSeeds;

public static class ProjectSeeds
{
    private static int _counter = 0;
    private static int ProjectCounter() => ++_counter;
    public static ProjectDetailModel ProjectSeed() => new()
    {
        Id = Guid.NewGuid(),
        Name = $"Project number {ProjectCounter()}",
        Description = "random project"
    };
}