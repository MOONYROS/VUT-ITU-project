using Microsoft.IdentityModel.Tokens;
using project.BL.Facades;
using project.BL.Facades.Interfaces;
using project.BL.Models;
using project.BL.tests.ModelSeeds;
using project.DAL.Tests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace project.BL.tests;

public class ActivityFacadeTests : FacadeTestsBase
{
    private readonly UserFacade _userFacade;
    private readonly ProjectFacade _projectFacade;
    private readonly ActivityFacade _activityFacade;

    public ActivityFacadeTests(ITestOutputHelper output) : base(output)
    {
        _userFacade = new UserFacade(UnitOfWorkFactory, UserModelMapper);
        _projectFacade = new ProjectFacade(UnitOfWorkFactory, ProjectModelMapper);
        _activityFacade = new ActivityFacade(UnitOfWorkFactory, ActivityModelMapper);
    }


    [Fact]
    public async Task CreateActivity_Success()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity = ActivitySeeds.ActivitySeed();

        // Act
        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedActivity = await _activityFacade.SaveAsync(activity, returnedUser.Id, null);

        var DbActivity = await _activityFacade.GetAsync(returnedActivity.Id);

        // Assert
        Assert.NotNull(DbActivity);
        Assert.Null(DbActivity.Project);
        Assert.NotNull(DbActivity.Tags);
        Assert.True(DbActivity.Tags.IsNullOrEmpty());
        DeepAssert.Equal(returnedActivity, DbActivity);
    }

    
    [Fact]
    public async Task CreateActivityWithProject_Success()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity = ActivitySeeds.ActivitySeed();
        var project = ProjectSeeds.ProjectSeed();

        // Act
        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedProject = await _projectFacade.SaveAsync(project);
        var returnedActivity = await _activityFacade.SaveAsync(activity, returnedUser.Id, returnedProject.Id);

        var DbActivity = await _activityFacade.GetAsync(returnedActivity.Id);
        var DbProject = await _projectFacade.GetAsync(returnedProject.Id);
        Assert.NotNull(DbActivity);
        Assert.NotNull(DbActivity.Project);
        Assert.NotNull(DbProject);
        var projectListModel = new ProjectListModel()
        {
            Id = DbProject.Id,
            Name = DbProject.Name
        };

        // Assert
        Assert.NotNull(DbActivity.Project);
        Assert.Equal(DbProject.Id, DbActivity.Project.Id);
        DeepAssert.Equal(DbActivity.Project, projectListModel);
    }


    [Fact]
    public async Task RemoveProjectFromActivity_Success()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity = ActivitySeeds.ActivitySeed();
        var project = ProjectSeeds.ProjectSeed();

        // Act
        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedProject = await _projectFacade.SaveAsync(project);
        var returnedActivity = await _activityFacade.SaveAsync(activity, returnedUser.Id, returnedProject.Id);

        var DbActivity = await _activityFacade.GetAsync(returnedActivity.Id);
        var DbProject = await _projectFacade.GetAsync(returnedProject.Id);

        Assert.NotNull(DbActivity);
        Assert.NotNull(DbActivity.Project);
        Assert.NotNull(DbProject);

        // Assert Bonded
        Assert.Equal(DbProject.Id, DbActivity.Project.Id);

        // Remove bond
        await _activityFacade.SaveAsync(DbActivity, returnedUser.Id, null);

        // Update
        DbActivity = await _activityFacade.GetAsync(DbActivity.Id);
        DbProject = await _projectFacade.GetAsync(returnedProject.Id);

        // Assert bond removed, project still exists
        Assert.NotNull(DbActivity);
        Assert.Null(DbActivity.Project);
        Assert.NotNull(DbProject);
    }

    // az bude setnull v dbcontextu
    /*
    [Fact]
    public async Task DeleteProjectInActivity()
    {

    }

    /*
    [Fact]
    public async Task DeleteProjectInMoreActivities()
    {

    }
    */

    [Fact]
    public async Task OneProject_MoreActivities()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity1 = ActivitySeeds.ActivitySeed();
        var activity2 = ActivitySeeds.ActivitySeed();
        var project = ProjectSeeds.ProjectSeed();

        activity1.DateTimeFrom = new DateTime(2021, 05, 15, 18, 00, 00);
        activity1.DateTimeTo = new DateTime(2021, 05, 15, 20, 00, 00);

        activity2.DateTimeFrom = new DateTime(2021, 05, 16, 20, 30, 00);
        activity2.DateTimeTo = new DateTime(2021, 05, 16, 22, 00, 00);


        // Act
        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedProject = await _projectFacade.SaveAsync(project);
        var returnedActivity1 = await _activityFacade.SaveAsync(activity1, returnedUser.Id, returnedProject.Id);
        var returnedActivity2 = await _activityFacade.SaveAsync(activity2, returnedUser.Id, returnedProject.Id);

        var DbActivity1 = await _activityFacade.GetAsync(returnedActivity1.Id);
        var DbActivity2 = await _activityFacade.GetAsync(returnedActivity2.Id);
        var DbProject = await _projectFacade.GetAsync(returnedProject.Id);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var projectListModel = new ProjectListModel()
        {
            Id = DbProject.Id,
            Name = DbProject.Name
        };
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        // Assert
        Assert.NotNull(DbActivity1);
        Assert.NotNull(DbActivity1.Project);
        Assert.Equal(DbProject.Id, DbActivity1.Project.Id);
        DeepAssert.Equal(DbActivity1.Project, projectListModel);

        Assert.NotNull(DbActivity2);
        Assert.NotNull(DbActivity2.Project);
        Assert.Equal(DbProject.Id, DbActivity2.Project.Id);
        DeepAssert.Equal(DbActivity2.Project, projectListModel);


        // Remove project from one activity
        await _activityFacade.SaveAsync(DbActivity1, returnedUser.Id, null);

        // Update
        DbActivity1 = await _activityFacade.GetAsync(DbActivity1.Id);

        // Assert
        Assert.NotNull(DbActivity1);
        Assert.Null(DbActivity1.Project);

        Assert.NotNull(DbActivity2);
        Assert.NotNull(DbActivity2.Project);
        Assert.Equal(DbProject.Id, DbActivity2.Project.Id);
        DeepAssert.Equal(DbActivity2.Project, projectListModel);
    }


    [Fact]
    public async Task DeleteAcitivty()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity = ActivitySeeds.ActivitySeed();

        // Act
        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedActivity = await _activityFacade.SaveAsync(activity, returnedUser.Id, null);

        var DbActivity = await _activityFacade.GetAsync(returnedActivity.Id);
        Assert.NotNull(DbActivity);

        await _activityFacade.DeleteAsync(returnedActivity.Id);

        // Assert
        DbActivity = await _activityFacade.GetAsync(returnedActivity.Id);
        Assert.Null(DbActivity);
    }


    [Fact]
    public async Task DeleteNonExistingAcitivty()
    {
        // Arrange
        var activity = ActivitySeeds.ActivitySeed();

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _activityFacade.DeleteAsync(activity.Id));
    }


    [Fact]
    public async Task DeleteAcitivty_WithProject()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity = ActivitySeeds.ActivitySeed();
        var project = ProjectSeeds.ProjectSeed();

        // Act
        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedProject = await _projectFacade.SaveAsync(project);
        var returnedActivity = await _activityFacade.SaveAsync(activity, returnedUser.Id, returnedProject.Id);

        var DbActivity = await _activityFacade.GetAsync(returnedActivity.Id);
        Assert.NotNull(DbActivity);
        Assert.NotNull(DbActivity.Project);

        await _activityFacade.DeleteAsync(returnedActivity.Id);

        var DbProject = await _projectFacade.GetAsync(returnedProject.Id);
        DbActivity = await _activityFacade.GetAsync(returnedActivity.Id);

        // Assert
        Assert.Null(DbActivity);
        Assert.NotNull(DbProject);
    }


    [Fact]
    public async Task DeleteUserWithAcitivty()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity = ActivitySeeds.ActivitySeed();

        // Act
        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedActivity = await _activityFacade.SaveAsync(activity, returnedUser.Id, null);

        var DbActivity = await _activityFacade.GetAsync(returnedActivity.Id);
        Assert.NotNull(DbActivity);

        await _userFacade.DeleteAsync(returnedUser.Id);

        // Assert
        DbActivity = await _activityFacade.GetAsync(returnedActivity.Id);
        var DbUser = await _userFacade.GetAsync(returnedUser.Id);
        Assert.Null(DbUser);
        Assert.Null(DbActivity);
    }


    [Fact]
    public async Task DeleteUser_WithAcitivty_WithProject()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity = ActivitySeeds.ActivitySeed();
        var project = ProjectSeeds.ProjectSeed();

        // Act
        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedProject = await _projectFacade.SaveAsync(project);
        var returnedActivity = await _activityFacade.SaveAsync(activity, returnedUser.Id, returnedProject.Id);

        // Check
        var DbActivity = await _activityFacade.GetAsync(returnedActivity.Id);
        Assert.NotNull(DbActivity);
        Assert.NotNull(DbActivity.Project);

        // Delete
        await _userFacade.DeleteAsync(returnedUser.Id);

        // Update
        DbActivity = await _activityFacade.GetAsync(returnedActivity.Id);
        var DbUser = await _userFacade.GetAsync(returnedUser.Id);
        var DbProject = await _projectFacade.GetAsync(returnedProject.Id); 
        
        // Assert
        Assert.Null(DbUser);
        Assert.Null(DbActivity);
        Assert.NotNull(DbProject);
    }


    [Fact]
    public async Task GetList_OneActivity_DeepAssert()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity = ActivitySeeds.ActivitySeed();

        // Act
        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedActivity = await _activityFacade.SaveAsync(activity, returnedUser.Id, null);

        // Check
        var DbActivity = await _activityFacade.GetAsync(returnedActivity.Id);
        Assert.NotNull(DbActivity);
        Assert.Null(DbActivity.Project);

        var _activityListModel = new ActivityListModel()
        {
            Id = DbActivity.Id,
            Name = DbActivity.Name,
            DateTimeFrom = DbActivity.DateTimeFrom,
            DateTimeTo = DbActivity.DateTimeTo,
            Color = DbActivity.Color,
            Tags = DbActivity.Tags,
            Project = DbActivity.Project
        };

        var activityList = await _activityFacade.GetAsyncUser(returnedUser.Id);
        Assert.True(activityList.Any());
        Assert.True(activityList.Count() == 1);
        DeepAssert.Equal(_activityListModel, activityList.First());
    }


    [Fact]
    public async Task GetList_Empty()
    {
        // Arrange
        var user = UserSeeds.UserSeed();

        // Act
        var returnedUser = await _userFacade.SaveAsync(user);

        // Assert
        var activityList = await _activityFacade.GetAsyncUser(returnedUser.Id);
        Assert.NotNull(activityList);
        Assert.True(activityList.IsNullOrEmpty());
    }


    [Fact]
    public async Task GetList_MoreActivites()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity1 = ActivitySeeds.ActivitySeed();
        var activity2 = ActivitySeeds.ActivitySeed();
        var activity3 = ActivitySeeds.ActivitySeed();

        activity1.DateTimeFrom = new DateTime(2021, 05, 15, 18, 00, 00);
        activity1.DateTimeTo = new DateTime(2021, 05, 15, 20, 00, 00);

        activity2.DateTimeFrom = new DateTime(2021, 05, 15, 20, 30, 00);
        activity2.DateTimeTo = new DateTime(2021, 05, 15, 22, 00, 00);

        activity3.DateTimeFrom = new DateTime(2021, 05, 16, 20, 30, 00);
        activity3.DateTimeTo = new DateTime(2021, 05, 16, 22, 00, 00);

        // Act
        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedActivity1 = await _activityFacade.SaveAsync(activity1, returnedUser.Id, null);
        var returnedActivity2 = await _activityFacade.SaveAsync(activity2, returnedUser.Id, null);
        var returnedActivity3 = await _activityFacade.SaveAsync(activity3, returnedUser.Id, null);

        var DbActivity1 = await _activityFacade.GetAsync(returnedActivity1.Id);
        var DbActivity2 = await _activityFacade.GetAsync(returnedActivity2.Id);
        var DbActivity3 = await _activityFacade.GetAsync(returnedActivity3.Id);


        var activityList = await _activityFacade.GetAsyncUser(returnedUser.Id);

        Assert.True(activityList.Any());

        List<Guid> Guids = new List<Guid>();
        foreach (var activity in activityList)
        {
            Guids.Add(activity.Id);
        }

        Assert.Equal(3, activityList.Count());
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        Assert.Contains(DbActivity1.Id, Guids);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        Assert.Contains(DbActivity2.Id, Guids);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        Assert.Contains(DbActivity3.Id, Guids);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

    
    [Fact]
    public async Task MoreUsers_MoreActivities()
    {
        // Arrange
        var user1 = UserSeeds.UserSeed();
        var user2 = UserSeeds.UserSeed();
        var activity1 = ActivitySeeds.ActivitySeed();
        var activity2 = ActivitySeeds.ActivitySeed();

        activity1.DateTimeFrom = new DateTime(2021, 05, 15, 18, 00, 00);
        activity1.DateTimeTo = new DateTime(2021, 05, 15, 20, 00, 00);

        activity2.DateTimeFrom = new DateTime(2021, 05, 16, 20, 30, 00);
        activity2.DateTimeTo = new DateTime(2021, 05, 16, 22, 00, 00);

        // Act
        var returnedUser1 = await _userFacade.SaveAsync(user1);
        var returnedUser2 = await _userFacade.SaveAsync(user2);
        var usr1Act1 = await _activityFacade.SaveAsync(activity1, returnedUser1.Id, null);
        var usr1Act2 = await _activityFacade.SaveAsync(activity2, returnedUser1.Id, null);
        var usr2Act1 = await _activityFacade.SaveAsync(activity1, returnedUser2.Id, null);
        var usr2Act2 = await _activityFacade.SaveAsync(activity2, returnedUser2.Id, null);

        var DbActivity1 = await _activityFacade.GetAsync(usr1Act1.Id);
        var DbActivity2 = await _activityFacade.GetAsync(usr1Act2.Id);
        var DbActivity3 = await _activityFacade.GetAsync(usr2Act1.Id);
        var DbActivity4 = await _activityFacade.GetAsync(usr2Act2.Id);


        var activityList1 = await _activityFacade.GetAsyncUser(returnedUser1.Id);
        var activityList2 = await _activityFacade.GetAsyncUser(returnedUser2.Id);

        Assert.True(activityList1.Any());
        Assert.True(activityList2.Any());

        List<Guid> Guids1 = new List<Guid>();
        List<Guid> Guids2 = new List<Guid>();
        foreach (var activity in activityList1)
        {
            Guids1.Add(activity.Id);
        }
        foreach (var activity in activityList2)
        {
            Guids2.Add(activity.Id);
        }

        Assert.Equal(2, activityList1.Count());
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        Assert.Contains(DbActivity1.Id, Guids1);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        Assert.Contains(DbActivity2.Id, Guids1);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        Assert.Equal(2, activityList2.Count());
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        Assert.Contains(DbActivity3.Id, Guids2);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        Assert.Contains(DbActivity4.Id, Guids2);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }
    

    [Fact]
    public async Task Overlapping_Activites_Exception_1()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity1 = ActivitySeeds.ActivitySeed();
        var activity2 = ActivitySeeds.ActivitySeed();

        activity1.DateTimeFrom = new DateTime(2021, 05, 15, 18, 00, 00);
        activity1.DateTimeTo = new DateTime(2021, 05, 15, 20, 00, 00);

        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedActivity1 = await _activityFacade.SaveAsync(activity1, returnedUser.Id, null);

        var DbActivity1 = await _activityFacade.GetAsync(returnedActivity1.Id);


        activity2.DateTimeFrom = new DateTime(2021, 05, 15, 18, 00, 00);
        activity2.DateTimeTo = new DateTime(2021, 05, 15, 20, 00, 00);


        await Assert.ThrowsAsync<OverlappingException>(async () => await _activityFacade.SaveAsync(activity2, returnedUser.Id, null));
    }


    [Fact]
    public async Task Overlapping_Activites_Exception_2()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity1 = ActivitySeeds.ActivitySeed();
        var activity2 = ActivitySeeds.ActivitySeed();

        activity1.DateTimeFrom = new DateTime(2021, 05, 15, 18, 00, 00);
        activity1.DateTimeTo = new DateTime(2021, 05, 15, 20, 00, 00);

        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedActivity1 = await _activityFacade.SaveAsync(activity1, returnedUser.Id, null);

        var DbActivity1 = await _activityFacade.GetAsync(returnedActivity1.Id);

        activity2.DateTimeFrom = new DateTime(2021, 05, 15, 17, 00, 00);
        activity2.DateTimeTo = new DateTime(2021, 05, 15, 19, 00, 00);

        await Assert.ThrowsAsync<OverlappingException>(async () => await _activityFacade.SaveAsync(activity2, returnedUser.Id, null));
    }


    [Fact]
    public async Task Overlapping_Activites_Exception_3()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity1 = ActivitySeeds.ActivitySeed();
        var activity2 = ActivitySeeds.ActivitySeed();

        activity1.DateTimeFrom = new DateTime(2021, 05, 15, 18, 00, 00);
        activity1.DateTimeTo = new DateTime(2021, 05, 15, 20, 00, 00);

        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedActivity1 = await _activityFacade.SaveAsync(activity1, returnedUser.Id, null);

        var DbActivity1 = await _activityFacade.GetAsync(returnedActivity1.Id);

        activity2.DateTimeFrom = new DateTime(2021, 05, 15, 19, 00, 00);
        activity2.DateTimeTo = new DateTime(2021, 05, 15, 21, 00, 00);

        await Assert.ThrowsAsync<OverlappingException>(async () => await _activityFacade.SaveAsync(activity2, returnedUser.Id, null));
    }


    [Fact]
    public async Task Overlapping_Activites_Exception_4()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity1 = ActivitySeeds.ActivitySeed();
        var activity2 = ActivitySeeds.ActivitySeed();

        activity1.DateTimeFrom = new DateTime(2021, 05, 15, 18, 00, 00);
        activity1.DateTimeTo = new DateTime(2021, 05, 15, 20, 00, 00);

        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedActivity1 = await _activityFacade.SaveAsync(activity1, returnedUser.Id, null);

        var DbActivity1 = await _activityFacade.GetAsync(returnedActivity1.Id);


        activity2.DateTimeFrom = new DateTime(2021, 05, 15, 18, 30, 00);
        activity2.DateTimeTo = new DateTime(2021, 05, 15, 19, 30, 00);

        await Assert.ThrowsAsync<OverlappingException>(async () => await _activityFacade.SaveAsync(activity2, returnedUser.Id, null));
    }


    [Fact]
    public async Task Overlapping_Activites_Exception_5()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity1 = ActivitySeeds.ActivitySeed();
        var activity2 = ActivitySeeds.ActivitySeed();

        activity1.DateTimeFrom = new DateTime(2021, 05, 15, 18, 00, 00);
        activity1.DateTimeTo = new DateTime(2021, 05, 15, 20, 00, 00);

        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedActivity1 = await _activityFacade.SaveAsync(activity1, returnedUser.Id, null);

        var DbActivity1 = await _activityFacade.GetAsync(returnedActivity1.Id);

        activity2.DateTimeFrom = new DateTime(2021, 05, 15, 17, 30, 00);
        activity2.DateTimeTo = new DateTime(2021, 05, 15, 20, 30, 00);

        await Assert.ThrowsAsync<OverlappingException>(async () => await _activityFacade.SaveAsync(activity2, returnedUser.Id, null));
    }


    [Fact]
    public async Task Overlapping_Activites_Update_NoException()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity1 = ActivitySeeds.ActivitySeed();
        var activity2 = ActivitySeeds.ActivitySeed();

        activity1.DateTimeFrom = new DateTime(2021, 05, 15, 18, 00, 00);
        activity1.DateTimeTo = new DateTime(2021, 05, 15, 20, 00, 00);

        activity2.DateTimeFrom = new DateTime(2021, 05, 15, 21, 00, 00);
        activity2.DateTimeTo = new DateTime(2021, 05, 15, 22, 00, 00);

        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedActivity1 = await _activityFacade.SaveAsync(activity1, returnedUser.Id, null);
        var returnedActivity2 = await _activityFacade.SaveAsync(activity2, returnedUser.Id, null);

        var DbActivity1 = await _activityFacade.GetAsync(returnedActivity1.Id);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        DbActivity1.DateTimeFrom = new DateTime(2021, 05, 15, 18, 30, 00);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        DbActivity1.DateTimeTo = new DateTime(2021, 05, 15, 20, 30, 00);

        await _activityFacade.SaveAsync(DbActivity1, returnedUser.Id, null);
    }


    [Fact]
    public async Task Overlapping_Activites_Update_Exception()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity1 = ActivitySeeds.ActivitySeed();
        var activity2 = ActivitySeeds.ActivitySeed();

        activity1.DateTimeFrom = new DateTime(2021, 05, 15, 18, 00, 00);
        activity1.DateTimeTo = new DateTime(2021, 05, 15, 20, 00, 00);

        activity2.DateTimeFrom = new DateTime(2021, 05, 15, 20, 30, 00);
        activity2.DateTimeTo = new DateTime(2021, 05, 15, 22, 00, 00);

        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedActivity1 = await _activityFacade.SaveAsync(activity1, returnedUser.Id, null);
        var returnedActivity2 = await _activityFacade.SaveAsync(activity2, returnedUser.Id, null);

        var DbActivity1 = await _activityFacade.GetAsync(returnedActivity1.Id);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        DbActivity1.DateTimeTo = new DateTime(2021, 05, 15, 21, 00 ,00);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        await Assert.ThrowsAsync<OverlappingException>(async () => await _activityFacade.SaveAsync(DbActivity1, returnedUser.Id, null));
    }


    [Fact]
    public async Task Overlapping_MoreUsers_Success()
    {
        // Arrange
        var user1 = UserSeeds.UserSeed();
        var user2 = UserSeeds.UserSeed();
        var activity1 = ActivitySeeds.ActivitySeed();
        var activity2 = ActivitySeeds.ActivitySeed();

        activity1.DateTimeFrom = new DateTime(2021, 05, 15, 18, 00, 00);
        activity1.DateTimeTo = new DateTime(2021, 05, 15, 20, 00, 00);

        activity2.DateTimeFrom = new DateTime(2021, 05, 15, 19, 00, 00);
        activity2.DateTimeTo = new DateTime(2021, 05, 15, 21, 00, 00);


        // Act
        var returnedUser1 = await _userFacade.SaveAsync(user1);
        var returnedUser2 = await _userFacade.SaveAsync(user2);
        var returnedActivity1 = await _activityFacade.SaveAsync(activity1, returnedUser1.Id, null);
        var returnedActivity2 = await _activityFacade.SaveAsync(activity2, returnedUser2.Id, null);
    }

        /*
        [Fact]
        public async Task DateFilter()
        {

        }

        /*
        [Fact]
        public async InteralFilter()
        {

        }

        /*
        [Fact]
        public async Task()
        {

        }

        /*
        [Fact]
        public async Task()
        {

        }

        */
    }

