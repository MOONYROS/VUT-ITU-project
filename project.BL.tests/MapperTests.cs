using System.Drawing;
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
        activityTagListMapper.AddTagToActivity_Models(refTag, mappedActivity);
        
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
        // Entities
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

    [Fact]
    public void MapActivityWithTags_ModelToEntity()
    {
        // Models
        var user = ModelSeeds.UserSeeds.UserSeed();
        var activity = ModelSeeds.ActivitySeeds.ActivityDetailSeed();
        var tag1 = ModelSeeds.TagSeeds.TagDetailSeed();
        var tag2 = ModelSeeds.TagSeeds.TagDetailSeed();
        
        var tag1InActivity = new ActivityTagDetailModel
        {
            Id = Guid.NewGuid(),
            ActivityId = activity.Id,
            TagId = tag1.Id
        };
        var tag2InActivity = new ActivityTagDetailModel
        {
            Id = Guid.NewGuid(),
            ActivityId = activity.Id,
            TagId = tag2.Id
        };
        activity.Tags.Add(tag1);
        activity.Tags.Add(tag2);

        // Mappers
        var activityMapper = new ActivityModelMapper();
        var tagMapper = new TagModelMapper();
        var activityTagMapper = new ActivityTagModelMapper();

        // Mapping to entities
        var mappedActivity = activityMapper.MapToEntity(activity, user.Id, null);
        var mappedTag1 = tagMapper.MapToEntity(tag1);
        var mappedTag2 = tagMapper.MapToEntity(tag2);
        var mappedTag1InActivity = activityTagMapper.MapToEntity(activity, tag1);
        var mappedTag2InActivity = activityTagMapper.MapToEntity(activity, tag2);
        
        // Adding tags to activities
        activityTagMapper.AddTagToActivity_Entities(mappedActivity, mappedTag1, mappedTag1InActivity);
        activityTagMapper.AddTagToActivity_Entities(mappedActivity, mappedTag2, mappedTag2InActivity);
        
        // Reference activities and tags
        var refActivity = new ActivityEntity
        {
            Id = activity.Id,
            DateTimeFrom = activity.DateTimeFrom,
            DateTimeTo = activity.DateTimeTo,
            Name = activity.Name,
            Description = activity.Description,
            Color = activity.Color.ToArgb(),
            Project = null,
            ProjectId = null,
            User = null,
            UserId = user.Id
        };
        var refTag1InActivity = new ActivityTagListEntity
        {
            // The Id of reference tag has to be the same as mapped, but it doesn't affect the quality of tests at all
            Id = mappedTag1InActivity.Id,
            ActivityId = tag1InActivity.ActivityId,
            Activity = null,
            TagId = tag1InActivity.TagId,
            Tag = null
        };
        var refTag2InActivity = new ActivityTagListEntity
        {
            Id = mappedTag2InActivity.Id,
            ActivityId = tag2InActivity.ActivityId,
            Activity = null,
            TagId = tag2InActivity.TagId,
            Tag = null
        };
        var refTag1 = new TagEntity
        {
            Id = tag1.Id,
            Name = tag1.Name,
            Color = tag1.Color.ToArgb(),
        };
        var refTag2 = new TagEntity
        {
            Id = tag2.Id,
            Name = tag2.Name,
            Color = tag2.Color.ToArgb(),
        };
        refActivity.Tags.Add(refTag1InActivity);
        refTag1.Activities.Add(refTag1InActivity);
        refActivity.Tags.Add(refTag2InActivity);
        refTag2.Activities.Add(refTag2InActivity);
        

        // Asserts
        DeepAssert.Equal(refActivity, mappedActivity);
        DeepAssert.Equal(refTag1, mappedTag1);
        DeepAssert.Equal(refTag2, mappedTag2);
        DeepAssert.Equal(refTag1InActivity, mappedTag1InActivity);
        DeepAssert.Equal(refTag2InActivity, mappedTag2InActivity);
    }
}