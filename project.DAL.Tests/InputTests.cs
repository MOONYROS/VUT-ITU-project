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
            user1.Projects.Add(user1InProject);
            user2.Projects.Add(user2InProject);

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

        [Fact]
        public async Task AddTwoActivitiesToProjectCollection()
        {
            var user = UserSeeds.UserSeed();
            
            var project = ProjectSeeds.ProjectSeed() with
            {
                Activities = new List<ActivityEntity>()
            };

            var activity1InProject = new ActivityEntity
            {
                Id = Guid.NewGuid(),
                DateTimeFrom = default,
                DateTimeTo = default,
                Name = "Activity 1",
                Color = 16711935,
                Project = project,
                ProjectId = project.Id,
                User = user,
                UserId = user.Id
            };
            var activity2InProject = new ActivityEntity
            {
                Id = Guid.NewGuid(),
                DateTimeFrom = default,
                DateTimeTo = default,
                Name = "Activity 2",
                Color = 16738740,
                Project = project,
                ProjectId = project.Id,
                User = user,
                UserId = user.Id
            };
            
            project.Activities.Add(activity1InProject);
            project.Activities.Add(activity2InProject);

            ProjectDbContextSUT.Users.Add(user);
            ProjectDbContextSUT.Projects.Add(project);
            await ProjectDbContextSUT.SaveChangesAsync();

            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var dbProject = await dbx.Projects.Include(i => i.Activities).SingleAsync(i => i.Id == project.Id);
            var dbActivity1 = await dbx.Activities.Include(i => i.Project).SingleAsync(i => i.Id == activity1InProject.Id);
            var dbActivity2 = await dbx.Activities.Include(i => i.Project).SingleAsync(i => i.Id == activity2InProject.Id);
            
            Assert.Equal(activity1InProject.User.Id, activity2InProject.User.Id);
            Assert.Equal(activity1InProject.ProjectId, activity2InProject.ProjectId);
            
            Assert.NotEqual(dbActivity1.Id, dbActivity2.Id);
            
            DeepAssert.Equal(project.Activities.Count, dbProject.Activities.Count);
        }

        // TODO nefunkfcni test, vypisuje 'Sequence has no elements'
        // Naposledy jsem to resil tak, ze elementy nebyly predany ProjectDbContextSUT, ale tady nevim.
        // Chova se to velmi podobne, ale myslim si, ze to nebude ten stejny problem.
        [Fact]
        public async Task AddTwoActivitiesToTwoTags()
        {
            var user = UserSeeds.UserSeed();
            
            var project = ProjectSeeds.ProjectSeed();
        
            var tag1 = TagSeeds.TagSeed();
            var tag2 = TagSeeds.TagSeed();
        
            var activity1 = ActivitySeeds.ActivitySeed() with
            {
                Project = project,
                ProjectId = project.Id,
                User = user,
                UserId = user.Id
            };
            var activity2 = ActivitySeeds.ActivitySeed() with
            {
                Project = project,
                ProjectId = project.Id,
                User = user,
                UserId = user.Id
            };
            
            var tagActivity1 = new ActivityTagListEntity
            {
                Id = Guid.NewGuid(),
                ActivityId = activity1.Id,
                TagId = tag1.Id
            };
            var tagActivity2 = new ActivityTagListEntity
            {
                Id = Guid.NewGuid(),
                ActivityId = activity2.Id,
                TagId = tag2.Id
            };
            
            activity1.Tags.Add(tagActivity1);
            activity2.Tags.Add(tagActivity2);
            tag1.Activities.Add(tagActivity1);
            tag2.Activities.Add(tagActivity2);
        
            ProjectDbContextSUT.Users.Add(user);
            ProjectDbContextSUT.Projects.Add(project);
            ProjectDbContextSUT.Tags.Add(tag1);
            ProjectDbContextSUT.Tags.Add(tag2);
            ProjectDbContextSUT.Activities.Add(activity1);
            ProjectDbContextSUT.Activities.Add(activity2);
            await ProjectDbContextSUT.SaveChangesAsync();
            
            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var dbProject = await dbx.Projects.Include(i => i.Activities).SingleAsync(i => i.Id == project.Id);
            var dbActivity1 = await dbx.Activities.Include(i => i.Tags).SingleAsync(i => i.Id == tagActivity1.Id); // pri prechodu z tohoto mista to crashuje
            var dbActivity2 = await dbx.Activities.Include(i => i.Tags).SingleAsync(i => i.Id == tagActivity2.Id);
            var dbTag1 = await dbx.Tags.Include(i => i.Activities).SingleAsync(i => i.Id == tagActivity1.Id);
            var dbTag2 = await dbx.Tags.Include(i => i.Activities).SingleAsync(i => i.Id == tagActivity2.Id);
            
            Assert.Equal(activity1.Id, activity1.Id);
        }
    }
}