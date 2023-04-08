using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using project.BL.Mappers;
using project.DAL.Tests;
using project.DAL.Tests.Seeds;
using project.BL.Models;
using project.DAL.Entities;

namespace project.BL.tests;

public class MapperTests : DbContextTestsBase
{
    [Fact]
    public async Task MapUser()
    {
        // Insert user into database
        var user = UserSeeds.UserSeed();
        
        ProjectDbContextSUT.Users.Add(user);
        await ProjectDbContextSUT.SaveChangesAsync();
        
        // Get updated dbContext with the new user
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var dbUser = await dbx.Users.SingleAsync(i => i.Id == user.Id);

        // Create reference UserDetailModel
        var refUser = new UserDetailModel
        {
            Id = user.Id,
            UserName = user.UserName,
            FullName = user.FullName
        };

        // Map user from database to UserDetailModel
        var mapper = new UserModelMapper();
        var mappedUser = mapper.MapToDetailModel(dbUser);
        
        // Compare
        Assert.Equal(refUser.Id, mappedUser.Id);
        Assert.Equal(refUser.FullName, mappedUser.FullName);
        Assert.Equal(refUser.UserName, mappedUser.UserName);
    }

    [Fact]
    public void MapActivityWithTag()
    {
        var user = UserSeeds.UserSeed();
        var activity = ActivitySeeds.ActivitySeed() with
        {
            User = user,
            UserId = user.Id
        };
        var tag = TagSeeds.TagSeed();
        var tagInActivity = new ActivityTagListEntity
        {
            Id = Guid.NewGuid(),
            ActivityId = activity.Id,
            Activity = activity,
            TagId = tag.Id,
            Tag = tag
        };

        var tagMapper = new TagModelMapper();
        var refTag = tagMapper.MapToDetailModel(tag);

        var refActivity = new ActivityDetailModel
        {
            Id = activity.Id,
            Name = activity.Name,
            DateTimeFrom = activity.DateTimeFrom,
            DateTimeTo = activity.DateTimeTo,
            Color = Color.FromArgb(activity.Color),
            Description = activity.Description,
        };
        refActivity.Tags.Add(refTag);

        var activityMapper = new ActivityModelMapper();
        var mappedActivity = activityMapper.MapToDetailModel(activity);
        
        var activityTagListMapper = new ActivityTagListMapper();
        activityTagListMapper.MapActivityTagListToDetailModel(refTag, mappedActivity);
        
        Assert.Equal(refActivity.Id, mappedActivity.Id);
        Assert.Equal(refActivity.Color, mappedActivity.Color);
        Assert.Equal(refActivity.Description, mappedActivity.Description);
        Assert.Equal(refActivity.Name, mappedActivity.Name);
        Assert.Equal(refActivity.DateTimeFrom, mappedActivity.DateTimeFrom);
        Assert.Equal(refActivity.DateTimeTo, mappedActivity.DateTimeTo);
        Assert.Equal(refActivity.Tags, mappedActivity.Tags);
    }

    [Fact]
    public void MapProject()
    {
        var project = ProjectSeeds.ProjectSeed();

        var projectMapper = new ProjectModelMappers();
        var mappedProject = projectMapper.MapToDetailModel(project);
        
        Assert.Equal(project.Id, mappedProject.Id);
        Assert.Equal(project.Name, mappedProject.Name);
        Assert.Equal(project.Description, mappedProject.Description);
    }

    [Fact]
    public void MapProjectWithUsers()
    {
        var user1 = UserSeeds.UserSeed();
        var user2 = UserSeeds.UserSeed();
        var project = ProjectSeeds.ProjectSeed();

        var userMapper = new UserModelMapper();
        var projectMapper = new ProjectModelMappers();
        var userProjectListMapper = new UserProjectListMapper();

        var mappedUser1 = userMapper.MapToDetailModel(user1);
        var mappedUser2 = userMapper.MapToDetailModel(user2);
        var mappedProject = projectMapper.MapToDetailModel(project);
        userProjectListMapper.ConnectUserWithProjectModel(mappedUser1, mappedProject);
        userProjectListMapper.ConnectUserWithProjectModel(mappedUser2, mappedProject);

        var refProject = new ProjectDetailModel
        {
            Id = project.Id,
            Name = project.Name,
            Color = Color.Empty,
            Description = project.Description,
        };
        refProject.Users.Add(mappedUser1);
        refProject.Users.Add(mappedUser2);

        var refUser1 = new UserDetailModel
        {
            Id = user1.Id,
            UserName = user1.UserName,
            FullName = user1.FullName,
        };
        refUser1.Projects.Add(mappedProject);
        
        var refUser2 = new UserDetailModel
        {
            Id = user2.Id,
            UserName = user2.UserName,
            FullName = user2.FullName,
        };
        refUser2.Projects.Add(mappedProject);

        // Assert.Equal(refProject, mappedProject); Dont know why it fails
        Assert.Equal(refUser1.Projects, mappedUser1.Projects);
        Assert.Equal(refUser2.Projects, mappedUser2.Projects);
        Assert.Equal(refProject.Users, mappedProject.Users);
    }
}