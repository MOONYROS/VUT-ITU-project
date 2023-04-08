using Microsoft.EntityFrameworkCore;
using project.DAL.Entities;
using project.DAL.Mappers;
using project.DAL.Repositories;
using project.DAL.Tests.Seeds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.DAL.Tests
{
    public class RepositoryTests : DbContextTestsBase
    {
        [Fact]
        public async Task RepositoryGetTest()
        {
            var user = UserSeeds.UserSeed();

            ProjectDbContextSUT.Users.Add(user);
            await ProjectDbContextSUT.SaveChangesAsync();

            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var dbEntity = await dbx.Users.SingleAsync(i => i.Id == user.Id);

            var Mapper = new UserEntityMapper();
            var Repo = new Repository<UserEntity>(dbx, Mapper);
            var Cokoliv = Repo.Get();
            var Result = Cokoliv.Where(i => i.Id == user.Id);
        }
    }
}
