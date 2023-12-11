using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using WpfApp1.DAL.Entities;
using WpfApp1.DAL.Tests.Seeds;

namespace WpfApp1.DAL.Tests;

public class DbContextTests : DbContextTestsBase
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
        var user = UserSeeds.UserSeed();
        var tag = TagSeeds.TagSeed() with { UserId = user.Id };

        ProjectDbContextSUT.Tags.Add(tag);
        ProjectDbContextSUT.Users.Add(user);
        await ProjectDbContextSUT.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var dbEntity = await dbx.Tags.SingleAsync(i => i.Id == tag.Id);

        Assert.Equal(dbEntity.Name, tag.Name);
        Assert.Equal(dbEntity.Color, tag.Color);
    }

    [Fact]
    public async Task AddNewUserWithTodo()
    {
        var user = UserSeeds.UserSeed();

        var todo = TodoSeeds.TodoSeed() with
        {
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
    public async Task AddTwoActivitiesToTwoTags()
    {
        var user = UserSeeds.UserSeed();
        var tag1 = TagSeeds.TagSeed() with { UserId = user.Id };
        var tag2 = TagSeeds.TagSeed() with { UserId = user.Id };

        var activity1 = ActivitySeeds.ActivitySeed();
        var activity2 = ActivitySeeds.ActivitySeed();
            
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
        var userActivity = new UserActivityListEntity
        {
	        Id = Guid.NewGuid(),
	        ActivityId = activity1.Id,
	        UserId = user.Id
        };
            
        // activity1.Tags.Add(tagActivity1);
        activity2.Tags.Add(tagActivity2);
        // tag1.Activities.Add(tagActivity1);
        tag2.Activities.Add(tagActivity2);

        activity1.Users.Add(userActivity);
        user.Activities.Add(userActivity);
        
        ProjectDbContextSUT.Users.Add(user);
        ProjectDbContextSUT.ATLists.Add(tagActivity1);
        ProjectDbContextSUT.Tags.Add(tag1);
        ProjectDbContextSUT.Tags.Add(tag2);
        ProjectDbContextSUT.Activities.Add(activity1);
        ProjectDbContextSUT.Activities.Add(activity2);
        await ProjectDbContextSUT.SaveChangesAsync();
            
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var dbActivity1 = await dbx.Activities.Include(i => i.Tags).ThenInclude(i=>i.Tag).ThenInclude(i=>i.User).Include(i=>i.Users).ThenInclude(i=>i.User).SingleAsync(i => i.Id == activity1.Id);
        // var dbActivity1 = await dbx.Activities.Include(i => i.Tags).SingleAsync(i => i.Id == activity1.Id);

        var dbActivity2 = await dbx.Activities.Include(i => i.Tags).Include(i=>i.Users).SingleAsync(i => i.Id == activity2.Id);
        var dbTag1 = await dbx.Tags.Include(i => i.Activities).SingleAsync(i => i.Id == tag1.Id);
        var dbTag2 = await dbx.Tags.Include(i => i.Activities).SingleAsync(i => i.Id == tag2.Id);

        var bro = await dbx.ATLists.SingleAsync(i => i.Id == tagActivity1.Id);
        
        Assert.Equal(activity1.Id, dbActivity1.Id);
        Assert.Equal(activity1.Color, dbActivity1.Color);
        Assert.Equal(activity1.DateTimeTo, dbActivity1.DateTimeTo);
        Assert.Equal(activity1.DateTimeFrom, dbActivity1.DateTimeFrom);
        Assert.Equal(activity1.Name, dbActivity1.Name);
        
        Assert.Equal(activity1.Tags.First().Tag.UserId, dbActivity1.Tags.First().Tag.UserId);

        Assert.Equal(bro.Id, tagActivity1.Id);
		    
        Assert.NotEqual(dbActivity1.Id, dbActivity2.Id);
        Assert.NotEqual(dbTag1.Id, dbTag2.Id);
            
        DeepAssert.Equal(activity1, dbActivity1);
    }

    [Fact]
    public async Task AddUserWithTodo_DeleteUser_TodoIsNull()
    {
        var user = UserSeeds.UserSeed();
        var todo = TodoSeeds.TodoSeed();
        user.Todos.Add(todo);

        ProjectDbContextSUT.Users.Add(user);
        ProjectDbContextSUT.Todos.Add(todo);
        await ProjectDbContextSUT.SaveChangesAsync();

        ProjectDbContextSUT.Remove(user);
        await ProjectDbContextSUT.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        TodoEntity? dbTodo;
        try
        {
            dbTodo = await dbx.Todos.SingleAsync(i => i.Id == todo.Id);
        }
        catch (InvalidOperationException)
        {
            dbTodo = null;
        }

        Assert.Null(dbTodo);
    }
    
    [Fact]
    public async Task AddUserAndTodo_DeleteTodo_UserPersists()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var todo = TodoSeeds.TodoSeed() with
        {
            UserId = user.Id
        };
        user.Todos.Add(todo);
        
        // Act
        ProjectDbContextSUT.Users.Add(user);
        ProjectDbContextSUT.Todos.Add(todo);
        await ProjectDbContextSUT.SaveChangesAsync();

        ProjectDbContextSUT.Remove(todo);
        await ProjectDbContextSUT.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();

        UserEntity? dbUser;
        try
        {
            dbUser = await dbx.Users.SingleAsync(i => i.Id == user.Id);
        }
        catch (InvalidOperationException)
        {
            dbUser = null;
        }

        // Assert
        Assert.NotNull(dbUser);
    }

    //TODO: koumedodelejtopls
    [Fact]
    public async Task AddUserTodo_DeleteUser_TodoDeleted()
    {
	    var user = UserSeeds.UserSeed();
	    var todo = TodoSeeds.TodoSeed() with
	    {
		    UserId = user.Id
	    };

	    ProjectDbContextSUT.Users.Add(user);
	    ProjectDbContextSUT.Todos.Add(todo);
	    await ProjectDbContextSUT.SaveChangesAsync();

	    ProjectDbContextSUT.Todos.Remove(todo);
	    await ProjectDbContextSUT.SaveChangesAsync();
		   
	    await using var dbx = await DbContextFactory.CreateDbContextAsync();
	    var dbUser = await dbx.Users.SingleAsync(i=>i.Id == user.Id);
	    
	    Assert.Equal(dbUser.Id, user.Id);
    }

    [Fact]
    public async Task AddActivityAndTag_DeleteActivity_TagPreserved_BindingDeleted()
    {
        // Arrange 
        var user = UserSeeds.UserSeed();
        var tag = TagSeeds.TagSeed() with
        {
            UserId = user.Id
        };
        var activity = ActivitySeeds.ActivitySeed();
        var binding = new ActivityTagListEntity
        {
            Id = Guid.NewGuid(),
            ActivityId = activity.Id,
            TagId = tag.Id
        };
        
        activity.Tags.Add(binding);
        tag.Activities.Add(binding);
        // tag.User = user;

        // Act
        ProjectDbContextSUT.Users.Add(user);
        ProjectDbContextSUT.Activities.Add(activity);
        ProjectDbContextSUT.Tags.Add(tag);
        ProjectDbContextSUT.ATLists.Add(binding);
        await ProjectDbContextSUT.SaveChangesAsync();

        ProjectDbContextSUT.Activities.Remove(activity);
        await ProjectDbContextSUT.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var dbTag = await dbx.Tags.Include(i => i.User).SingleAsync(i => i.Id == tag.Id);
        
        ActivityEntity? dbActivity;
        try
        {
            dbActivity = await dbx.Activities.SingleAsync(i => i.Id == activity.Id);
        }
        catch (InvalidOperationException)
        {
            dbActivity = null;
        }

        ActivityTagListEntity? dbAT;
        try
        {
            dbAT = await dbx.ATLists.SingleAsync(i => i.Id == binding.Id);
        }
        catch (InvalidOperationException)
        {
            dbAT = null;
        }
        
        // Assert
        Assert.Null(dbActivity);
        Assert.Null(dbAT);
        
        DeepAssert.Equal(tag, dbTag);
    }
    
    [Fact]
    public async Task AddActivityAndTag_DeleteTag_ActivityPreserved_BindingDeleted()
    {
        // Arrange 
        var user = UserSeeds.UserSeed();
        var tag = TagSeeds.TagSeed() with
        {
            UserId = user.Id
        };
        var activity = ActivitySeeds.ActivitySeed();
        var binding = new ActivityTagListEntity
        {
            Id = Guid.NewGuid(),
            ActivityId = activity.Id,
            TagId = tag.Id
        };
        
        activity.Tags.Add(binding);
        tag.Activities.Add(binding);

        // Act
        ProjectDbContextSUT.Users.Add(user);
        ProjectDbContextSUT.Activities.Add(activity);
        ProjectDbContextSUT.Tags.Add(tag);
        ProjectDbContextSUT.ATLists.Add(binding);
        await ProjectDbContextSUT.SaveChangesAsync();

        ProjectDbContextSUT.Tags.Remove(tag);
        await ProjectDbContextSUT.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        // var dbActivity = await dbx.Activities.Include(i => i.User).SingleAsync(i => i.Id == activity.Id);
        var dbActivity = await dbx.Activities.SingleAsync(i => i.Id == activity.Id);

        
        TagEntity? dbTag;
        try
        {
            dbTag = await dbx.Tags.SingleAsync(i => i.Id == tag.Id);
        }
        catch (InvalidOperationException)
        {
            dbTag = null;
        }

        ActivityTagListEntity? dbAT;
        try
        {
            dbAT = await dbx.ATLists.SingleAsync(i => i.Id == binding.Id);
        }
        catch (InvalidOperationException)
        {
            dbAT = null;
        }
        
        // Assert
        Assert.Null(dbTag);
        Assert.Null(dbAT);
        DeepAssert.Equal(activity, dbActivity);
    }

    [Fact]
    public async Task AddUserAndTag_DeleteUser_TagIsNull()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var tag = TagSeeds.TagSeed() with
        {
            UserId = user.Id
        };
        
        // Act
        ProjectDbContextSUT.Tags.Add(tag);
        ProjectDbContextSUT.Users.Add(user);
        await ProjectDbContextSUT.SaveChangesAsync();

        ProjectDbContextSUT.Remove(user);
        await ProjectDbContextSUT.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        TagEntity? dbTag;
        try
        {
            dbTag = await dbx.Tags.SingleAsync(i => i.Id == tag.Id);
        }
        catch (InvalidOperationException)
        {
            dbTag = null;
        }
        
        // Assert
        Assert.Null(dbTag);
    }

    [Fact]
    public async Task AddUserAndTag_DeleteTag_UserPersists()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var tag = TagSeeds.TagSeed() with
        {
            UserId = user.Id
        };

        // Act
        ProjectDbContextSUT.Tags.Add(tag);
        ProjectDbContextSUT.Users.Add(user);
        await ProjectDbContextSUT.SaveChangesAsync();
        
        ProjectDbContextSUT.Remove(tag);
        await ProjectDbContextSUT.SaveChangesAsync();
        
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        UserEntity? dbUser;
        try
        {
            dbUser = await dbx.Users.SingleAsync(i => i.Id == user.Id);
        }
        catch (InvalidOperationException)
        {
            dbUser = null;
        }
        
        // Assert
        Assert.NotNull(dbUser);
        DeepAssert.Equal(user, dbUser);
    }

    [Fact]
    public async Task AddUserAndActivity_DeleteActivity_UserPersists()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity = ActivitySeeds.ActivitySeed();

        // Act
        ProjectDbContextSUT.Activities.Add(activity);
        ProjectDbContextSUT.Users.Add(user);
        await ProjectDbContextSUT.SaveChangesAsync();
        
        ProjectDbContextSUT.Remove(activity);
        await ProjectDbContextSUT.SaveChangesAsync();
        
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        UserEntity? dbUser;
        try
        {
            dbUser = await dbx.Users.SingleAsync(i => i.Id == user.Id);
        }
        catch (InvalidOperationException)
        {
            dbUser = null;
        }
        
        // Assert
        Assert.NotNull(dbUser);
        DeepAssert.Equal(user, dbUser);
    }
}