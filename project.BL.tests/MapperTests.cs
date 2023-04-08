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
    public void MapUser_EntityToModel()
    {
        // Create new user entity
        var user = UserSeeds.UserSeed();

        // Create reference UserDetailModel
        var refUser = new UserDetailModel
        {
            Id = user.Id,
            UserName = user.UserName,
            FullName = user.FullName
        };

        // Map user entity to UserDetailModel
        var mapper = new UserModelMapper();
        var mappedUser = mapper.MapToDetailModel(user);
        
        // Compare
        DeepAssert.Equal(refUser, mappedUser);
    }

    [Fact]
    public void MapActivityWithTag_EntityToModel()
    {
        var user = UserSeeds.UserSeed();
        var activity = ActivitySeeds.ActivitySeed() with
        {
            User = user,
            UserId = user.Id
        };
        var tag = TagSeeds.TagSeed();

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
        
        var activityTagListMapper = new ActivityTagModelMapper();
        activityTagListMapper.AddTagToActivity(refTag, mappedActivity);
        
        DeepAssert.Equal(refActivity, mappedActivity);
    }

    [Fact]
    public void MapProject_EntityToModel()
    {
        var project = ProjectSeeds.ProjectSeed();

        var projectMapper = new ProjectModelMappers();
        var mappedProject = projectMapper.MapToDetailModel(project);
        
        Assert.Equal(project.Id, mappedProject.Id);
        Assert.Equal(project.Name, mappedProject.Name);
        Assert.Equal(project.Description, mappedProject.Description);
    }

    [Fact]
    public void MapProjectWithUsers_EntityToModel()
    {
        var user1 = UserSeeds.UserSeed();
        var user2 = UserSeeds.UserSeed();
        var project = ProjectSeeds.ProjectSeed();

        var userMapper = new UserModelMapper();
        var projectMapper = new ProjectModelMappers();
        var userProjectListMapper = new UserProjectModelMapper();

        var mappedUser1 = userMapper.MapToDetailModel(user1);
        var mappedUser2 = userMapper.MapToDetailModel(user2);
        var mappedProject = projectMapper.MapToDetailModel(project);
        userProjectListMapper.AddUserToProject(mappedUser1, mappedProject);
        userProjectListMapper.AddUserToProject(mappedUser2, mappedProject);

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

        DeepAssert.Equal(refProject, mappedProject);
        DeepAssert.Equal(refUser1, mappedUser1);
        DeepAssert.Equal(refUser2, mappedUser2);
    }
}