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
        
        var activity1 = ActivitySeeds.ActivitySeed() with
        {
            User = user,
            UserId = user.Id
        };
        var activity2 = ActivitySeeds.ActivitySeed() with
        {
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
        ProjectDbContextSUT.Tags.Add(tag1);
        ProjectDbContextSUT.Tags.Add(tag2);
        ProjectDbContextSUT.Activities.Add(activity1);
        ProjectDbContextSUT.Activities.Add(activity2);
        await ProjectDbContextSUT.SaveChangesAsync();
            
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var dbActivity1 = await dbx.Activities.Include(i => i.Tags).Include(i=>i.User).SingleAsync(i => i.Id == activity1.Id);
        var dbActivity2 = await dbx.Activities.Include(i => i.Tags).SingleAsync(i => i.Id == activity2.Id);
        var dbTag1 = await dbx.Tags.Include(i => i.Activities).SingleAsync(i => i.Id == tag1.Id);
        var dbTag2 = await dbx.Tags.Include(i => i.Activities).SingleAsync(i => i.Id == tag2.Id);
            
        Assert.Equal(activity1.Id, dbActivity1.Id);
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

   [Fact]
    public async Task AddActivityAndTag_DeleteActivity_TagPreserved_BindingDeleted()
    {
        // Arrange 
        var user = UserSeeds.UserSeed();
        var tag = TagSeeds.TagSeed() with
        {
            UserId = user.Id
        };
        var activity = ActivitySeeds.ActivitySeed() with
        {
            UserId = user.Id
        };
        var binding = new ActivityTagListEntity
        {
            Id = Guid.NewGuid(),
            ActivityId = activity.Id,
            TagId = tag.Id
        };
        
        activity.Tags.Add(binding);
        tag.Activities.Add(binding);
        tag.User = user;

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
        var activity = ActivitySeeds.ActivitySeed() with
        {
            UserId = user.Id
        };
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
        var dbActivity = await dbx.Activities.Include(i => i.User).SingleAsync(i => i.Id == activity.Id);
        
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
    public async Task AddUserAndActivity_DeleteUser_ActivityIsNull()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity = ActivitySeeds.ActivitySeed() with
        {
            UserId = user.Id
        };
        
        // Act
        ProjectDbContextSUT.Activities.Add(activity);
        ProjectDbContextSUT.Users.Add(user);
        await ProjectDbContextSUT.SaveChangesAsync();

        ProjectDbContextSUT.Remove(user);
        await ProjectDbContextSUT.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        ActivityEntity? dbActivity;
        try
        {
            dbActivity = await dbx.Activities.SingleAsync(i => i.Id == activity.Id);
        }
        catch (InvalidOperationException)
        {
            dbActivity = null;
        }
        
        // Assert
        Assert.Null(dbActivity);
    }
    
    [Fact]
    public async Task AddUserAndActivity_DeleteActivity_UserPersists()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity = ActivitySeeds.ActivitySeed() with
        {
            UserId = user.Id
        };

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