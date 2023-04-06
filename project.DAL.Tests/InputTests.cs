using System.Diagnostics;
using project.DAL;
using Microsoft.EntityFrameworkCore;
using project.DAL.Entities;
using project.DAL.Tests.Seeds;

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

        [Fact]
        public async Task AddNewUserWithTodo()
        {
            UserEntity user = new()
            {
                Id = Guid.NewGuid(),
                FullName = "Adam",
                UserName = "Malysak"
            };

            TodoEntity todo = new()
            {
                Id = Guid.NewGuid(),
                Name = "Spachat neziti",
                Date = DateOnly.Parse("January 1, 2000"),
                Finished = false,
                User = user,
                UserId = user.Id
            };

            ProjectDbContextSUT.Todos.Add(todo);
            ProjectDbContextSUT.Users.Add(user);
            await ProjectDbContextSUT.SaveChangesAsync();

            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var dbTodo = await dbx.Todos.SingleAsync(i => i.Id == todo.Id);

            Assert.Equal(dbTodo.Name, todo.Name);
            Assert.Equal(dbTodo.Date, todo.Date);
            Assert.Equal(dbTodo.Finished, todo.Finished);
        }

        [Fact]
        public async Task AddNewProjectWithActivity()
        {
            UserEntity user = new()
            {
                Id = Guid.NewGuid(),
                FullName = "Ondrej Koumar",
                UserName = "Koumy"
            };
            ProjectEntity project = new()
            {
                Id = Guid.NewGuid(),
                Name = "ICS projekt speedrun any%",
                Description = "set seed glitchless WR attempt"
            };

            ActivityEntity activity = ActivitySeeds.ActivitySeed() with
            {
                User = user,
                UserId = user.Id,
                Project = project,
                ProjectId = project.Id
            };

            ProjectDbContextSUT.Users.Add(user);
            ProjectDbContextSUT.Projects.Add(project);
            ProjectDbContextSUT.Activities.Add(activity);
            await ProjectDbContextSUT.SaveChangesAsync();

            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var dbActivity = await dbx.Activities.SingleAsync(i => i.Id == activity.Id);
            var dbProject = await dbx.Projects.SingleAsync(i => i.Id == project.Id);
            var dbUser = await dbx.Users.SingleAsync(i => i.Id == user.Id);

            Assert.Equal(user.Id, dbActivity.User.Id);
            Debug.Assert(dbActivity.Project != null, "dbActivity.Project != null");
            Assert.Equal(project.Id, dbActivity.Project.Id);
            Assert.Equal(user.Id, dbActivity.UserId);
            Assert.Equal(project.Id, dbActivity.ProjectId);
        }
    }
}