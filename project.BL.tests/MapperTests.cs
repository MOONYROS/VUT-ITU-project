using Microsoft.EntityFrameworkCore;
using project.DAL.Tests;
using project.DAL.Tests.Seeds;
using project.BL;

namespace project.BL.tests;

public class MapperTests : DbContextTestsBase
{
    [Fact]
    public async Task MapUser()
    {
        var user = UserSeeds.UserSeed();
        
        ProjectDbContextSUT.Users.Add(user);
        await ProjectDbContextSUT.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var dbUser = await dbx.Users.SingleAsync(i => i.Id == user.Id);
        
        
    }
}