using System.Diagnostics;
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
            var user = UserSeeds.UserSeed();
            
            ProjectDbContextSUT.Users.Add(user);
            await ProjectDbContextSUT.SaveChangesAsync();

            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var dbEntity = await dbx.Users.SingleAsync(i => i.Id == user.Id);

            Assert.Equal(dbEntity.FullName, user.FullName);
            Assert.Equal(dbEntity.UserName, user.UserName);
        }
        
        [Fact]
        public async Task AddNewTag()
        {
            var tag = TagSeeds.TagSeed();

            ProjectDbContextSUT.Tags.Add(tag);
            await ProjectDbContextSUT.SaveChangesAsync();

            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var dbEntity = await dbx.Tags.SingleAsync(i => i.Id == tag.Id);

            Assert.Equal(dbEntity.Name, tag.Name);
            Assert.Equal(dbEntity.Color, tag.Color);
        }
        
        [Fact]
        public async Task AddNewProject()
        {
            var project = ProjectSeeds.ProjectSeed();
            
            ProjectDbContextSUT.Projects.Add(project);
            await ProjectDbContextSUT.SaveChangesAsync();

            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var dbEntity = await dbx.Projects.SingleAsync(i => i.Id == project.Id);

            Assert.Equal(dbEntity.Name, project.Name);
            Assert.Equal(dbEntity.Description, project.Description);
        }

        [Fact]
        public async Task AddNewUserWithTodo()
        {
            var user = UserSeeds.UserSeed();

            var todo = TodoSeeds.TodoSeed() with
            {
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
            var user = UserSeeds.UserSeed();
            
            var project = ProjectSeeds.ProjectSeed();

            var activity = ActivitySeeds.ActivitySeed() with
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
            Assert.Equal(dbProject.Id, dbActivity.ProjectId);
            Assert.Equal(dbActivity.User.Id, dbUser.Id);
        }

        [Fact]
        public async Task AddTwoUsersDifferentProject()
        {
            var user1 = UserSeeds.UserSeed();
            var user2 = UserSeeds.UserSeed();
        
            var project1 = ProjectSeeds.ProjectSeed();
            var project2 = ProjectSeeds.ProjectSeed();

            var activity1 = ActivitySeeds.ActivitySeed() with
            {
                User = user1,
                UserId = user1.Id,
                Project = project1,
                ProjectId = project1.Id
            };
            var activity2 = ActivitySeeds.ActivitySeed() with
            {
                User = user2,
                UserId = user2.Id,
                Project = project2,
                ProjectId = project2.Id
            };
            
            ProjectDbContextSUT.Users.Add(user1);
            ProjectDbContextSUT.Users.Add(user2);
            ProjectDbContextSUT.Projects.Add(project1);
            ProjectDbContextSUT.Projects.Add(project2);
            ProjectDbContextSUT.Activities.Add(activity1);
            ProjectDbContextSUT.Activities.Add(activity2);
            await ProjectDbContextSUT.SaveChangesAsync();
            
            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var dbActivity1 = await dbx.Activities.SingleAsync(i => i.Id == activity1.Id);
            var dbActivity2 = await dbx.Activities.SingleAsync(i => i.Id == activity2.Id);
            var dbProject1 = await dbx.Projects.SingleAsync(i => i.Id == project1.Id);
            var dbProject2 = await dbx.Projects.SingleAsync(i => i.Id == project2.Id);
            var dbUser1 = await dbx.Users.SingleAsync(i => i.Id == user1.Id);
            var dbUser2 = await dbx.Users.SingleAsync(i => i.Id == user2.Id);
            
            Assert.Equal(dbActivity1.User.Id, user1.Id);
            Assert.Equal(dbActivity2.User.Id, user2.Id);
            Assert.Equal(dbUser1.Id, user1.Id);
            Assert.Equal(dbUser2.Id, user2.Id);

            Assert.NotEqual(dbProject1.Id, dbProject2.Id);
            Assert.NotEqual(dbUser1.Id, dbUser2.Id);
            Assert.NotEqual(dbUser1.Id, dbActivity2.Id);
            Assert.NotEqual(dbUser2.Id, dbActivity1.Id);
        }

        [Fact]
        public async Task AddTwoActivitiesToProject()
        {
            var user = UserSeeds.UserSeed();
            
            var project = ProjectSeeds.ProjectSeed();

            var activity1 = ActivitySeeds.ActivitySeed() with
            {
                User = user,
                UserId = user.Id,
                Project = project,
                ProjectId = project.Id
            };
            var activity2 = ActivitySeeds.ActivitySeed() with
            {
                User = user,
                UserId = user.Id,
                Project = project,
                ProjectId = project.Id
            };

            ProjectDbContextSUT.Users.Add(user);
            ProjectDbContextSUT.Projects.Add(project);
            ProjectDbContextSUT.Activities.Add(activity1);
            ProjectDbContextSUT.Activities.Add(activity2);
            await ProjectDbContextSUT.SaveChangesAsync();
            
            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var dbActivity1 = await dbx.Activities.SingleAsync(i => i.Id == activity1.Id);
            var dbActivity2 = await dbx.Activities.SingleAsync(i => i.Id == activity2.Id);
            var dbProject = await dbx.Projects.SingleAsync(i => i.Id == project.Id);
            var dbUser = await dbx.Users.SingleAsync(i => i.Id == user.Id);
            
            Assert.Equal(dbActivity1.User.Id, dbActivity2.User.Id);
            Assert.Equal(dbProject.Id, activity1.ProjectId);
            Assert.Equal(dbProject.Id, activity2.ProjectId);
            Assert.Equal(dbUser.Id, user.Id);
            
            Assert.NotEqual(dbActivity1.Id, activity2.Id);
        }

        [Fact]
        public async Task AddTwoUsersToProject()
        {
            var user1 = UserSeeds.UserSeed();
            var user2 = UserSeeds.UserSeed();

            var project = ProjectSeeds.ProjectSeed() with
            {
                Users = new List<UserProjectListEntity>()
            };

            var user1InProject = new UserProjectListEntity
            {
                Id = Guid.NewGuid(),
                ProjectId = project.Id,
                Project = project,
                UserId = user1.Id,
                User = user1
            };
            var user2InProject = new UserProjectListEntity
            {
                Id = Guid.NewGuid(),
                ProjectId = project.Id,
                Project = project,
                UserId = user2.Id,
                User = user2
            };
            
            project.Users.Add(user1InProject);
            project.Users.Add(user2InProject);
            user2.Projects.Add(user2InProject);
            user1.Projects.Add(user1InProject);
            
            ProjectDbContextSUT.Users.Add(user1);
            ProjectDbContextSUT.Users.Add(user2);
            ProjectDbContextSUT.Projects.Add(project);
            await ProjectDbContextSUT.SaveChangesAsync();
            
            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var dbProject = await dbx.Projects.Include(i => i.Users).SingleAsync(i => i.Id == project.Id);
            var dbUser1 = await dbx.Users.Include(i => i.Projects).ThenInclude(i => i.Project).SingleAsync(i => i.Id == user1.Id);
            var dbUser2 = await dbx.Users.Include(i => i.Projects).ThenInclude(i => i.Project).SingleAsync(i => i.Id == user2.Id);

            Assert.NotEqual(dbUser1.Id, dbUser2.Id);
            
            DeepAssert.Equal(project.Users, dbProject.Users);
            DeepAssert.Equal(project, dbProject);
        }
    }
}