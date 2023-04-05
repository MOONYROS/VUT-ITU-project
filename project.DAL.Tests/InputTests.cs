using project.DAL;
using Microsoft.EntityFrameworkCore;
using project.DAL.Entities;

namespace project.DAL.Tests
{
    public class InputTests : DbContextTestsBase
    {
        
        [Fact]

        public async Task AddNewUser()
        {
            UserEntity entity = new()
            {
                Id = Guid.NewGuid(),
                FullName = "Adam Malysak",
                UserName = "Malys"
            };

            ProjectDbContextSUT.Users.Add(entity);
            await ProjectDbContextSUT.SaveChangesAsync();

            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var dbEntity = await dbx.Users.SingleAsync(i => i.Id == entity.Id);

            Assert.Equal(dbEntity.FullName, entity.FullName);
            Assert.Equal(dbEntity.UserName, entity.UserName);
        }

        
        [Fact]
        public async Task AddNewTag()
        {
            TagEntity entity = new()
            {
                Id = Guid.NewGuid(),
                Name = "Odpoledni hrani",
                Color = 1
            };

            ProjectDbContextSUT.Tags.Add(entity);
            await ProjectDbContextSUT.SaveChangesAsync();

            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var dbEntity = await dbx.Tags.SingleAsync(i => i.Id == entity.Id);

            Assert.Equal(dbEntity.Name, entity.Name);
            Assert.Equal(dbEntity.Color, entity.Color);
        }
        

        /*
        [Fact]
        public async Task AddNewProject()
        {
            ProjectEntity entity = new()
            {
                Id = Guid.NewGuid(),
                Name = "Brno siege",
                Description= "Zazvon v 11"
            };

            ProjectDbContextSUT.Projects.Add(entity);
            await ProjectDbContextSUT.SaveChangesAsync();

            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var dbEntity = await dbx.Projects.SingleAsync(i => i.Id == entity.Id);

            Assert.Equal(dbEntity.Name, entity.Name);
            Assert.Equal(dbEntity.Description, entity.Description);
        }
        */
        

        /*
        [Fact]
        public async Task AddTodo()
        {
            TodoEntity entity = new()
            {
                Id = Guid.NewGuid(),
                Name = "Spachat neziti",
                Date = DateOnly.Parse("January 1, 2000"),
                Finished = false
            };

            ProjectDbContextSUT.Todos.Add(entity);
            await ProjectDbContextSUT.SaveChangesAsync();

            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var dbEntity = await dbx.Todos.SingleAsync(i => i.Id == entity.Id);

            Assert.Equal(dbEntity.Name, entity.Name);
            Assert.Equal(dbEntity.Date, entity.Date);
            Assert.Equal(dbEntity.Finished, entity.Finished);

        }*/
    }
}