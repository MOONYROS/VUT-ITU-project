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

        activity1.DateTimeFrom = DateTime.Parse("15/5/2021 18:00");
        activity1.DateTimeFrom = DateTime
        activity1.DateTimeTo = DateTime.Parse("15/5/2021 20:00");

        activity2.DateTimeFrom = DateTime.Parse("15/5/2021 20:30");
        activity2.DateTimeTo = DateTime.Parse("15/5/2021 22:00");

        activity3.DateTimeFrom = DateTime.Parse("16/5/2021 20:30");
        activity3.DateTimeTo = DateTime.Parse("16/5/2021 22:00");

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
        Assert.Contains(DbActivity1.Id, Guids);
        Assert.Contains(DbActivity2.Id, Guids);
        Assert.Contains(DbActivity3.Id, Guids);
    }

    /*
    [Fact]
    public async Task MoreUsers_MoreActivities()
    {

    }

    /*
    [Fact]
    public async Task OneProject_MoreActivities()
    {

    }
    */

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


        //activity2.DateTimeFrom = DateTime.Parse("15/5/2021 18:00");
        //activity2.DateTimeTo = DateTime.Parse("15/5/2021 20:00");
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

        activity1.DateTimeFrom = DateTime.Parse("15/5/2021 18:00");
        activity1.DateTimeTo = DateTime.Parse("15/5/2021 20:00");

        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedActivity1 = await _activityFacade.SaveAsync(activity1, returnedUser.Id, null);

        var DbActivity1 = await _activityFacade.GetAsync(returnedActivity1.Id);


        activity2.DateTimeFrom = DateTime.Parse("15/5/2021 17:00");
        activity2.DateTimeTo = DateTime.Parse("15/5/2021 19:00");

        await Assert.ThrowsAsync<OverlappingException>(async () => await _activityFacade.SaveAsync(activity2, returnedUser.Id, null));
    }


    [Fact]
    public async Task Overlapping_Activites_Exception_3()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity1 = ActivitySeeds.ActivitySeed();
        var activity2 = ActivitySeeds.ActivitySeed();

        activity1.DateTimeFrom = DateTime.Parse("15/5/2021 18:00");
        activity1.DateTimeTo = DateTime.Parse("15/5/2021 20:00");

        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedActivity1 = await _activityFacade.SaveAsync(activity1, returnedUser.Id, null);

        var DbActivity1 = await _activityFacade.GetAsync(returnedActivity1.Id);


        activity2.DateTimeFrom = DateTime.Parse("15/5/2021 19:00");
        activity2.DateTimeTo = DateTime.Parse("15/5/2021 21:00");

        await Assert.ThrowsAsync<OverlappingException>(async () => await _activityFacade.SaveAsync(activity2, returnedUser.Id, null));
    }


    [Fact]
    public async Task Overlapping_Activites_Exception_4()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity1 = ActivitySeeds.ActivitySeed();
        var activity2 = ActivitySeeds.ActivitySeed();

        activity1.DateTimeFrom = DateTime.Parse("15/5/2021 18:00");
        activity1.DateTimeTo = DateTime.Parse("15/5/2021 20:00");

        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedActivity1 = await _activityFacade.SaveAsync(activity1, returnedUser.Id, null);

        var DbActivity1 = await _activityFacade.GetAsync(returnedActivity1.Id);


        activity2.DateTimeFrom = DateTime.Parse("15/5/2021 18:30");
        activity2.DateTimeTo = DateTime.Parse("15/5/2021 19:30");

        await Assert.ThrowsAsync<OverlappingException>(async () => await _activityFacade.SaveAsync(activity2, returnedUser.Id, null));
    }


    [Fact]
    public async Task Overlapping_Activites_Exception_5()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity1 = ActivitySeeds.ActivitySeed();
        var activity2 = ActivitySeeds.ActivitySeed();

        activity1.DateTimeFrom = DateTime.Parse("15/5/2021 18:00");
        activity1.DateTimeTo = DateTime.Parse("15/5/2021 20:00");

        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedActivity1 = await _activityFacade.SaveAsync(activity1, returnedUser.Id, null);

        var DbActivity1 = await _activityFacade.GetAsync(returnedActivity1.Id);


        activity2.DateTimeFrom = DateTime.Parse("15/5/2021 17:30");
        activity2.DateTimeTo = DateTime.Parse("15/5/2021 20:30");

        await Assert.ThrowsAsync<OverlappingException>(async () => await _activityFacade.SaveAsync(activity2, returnedUser.Id, null));
    }


    [Fact]
    public async Task Overlapping_Activites_Update_NoException()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity1 = ActivitySeeds.ActivitySeed();
        var activity2 = ActivitySeeds.ActivitySeed();

        activity1.DateTimeFrom = DateTime.Parse("15/5/2021 18:00");
        activity1.DateTimeTo = DateTime.Parse("15/5/2021 20:00");

        activity2.DateTimeFrom = DateTime.Parse("15/5/2021 21:00");
        activity2.DateTimeTo = DateTime.Parse("15/5/2021 22:00");

        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedActivity1 = await _activityFacade.SaveAsync(activity1, returnedUser.Id, null);
        var returnedActivity2 = await _activityFacade.SaveAsync(activity2, returnedUser.Id, null);

        var DbActivity1 = await _activityFacade.GetAsync(returnedActivity1.Id);


        DbActivity1.DateTimeFrom = DateTime.Parse("15/5/2021 18:30");
        DbActivity1.DateTimeTo = DateTime.Parse("15/5/2021 20:30");

        await _activityFacade.SaveAsync(DbActivity1, returnedUser.Id, null);
    }


    [Fact]
    public async Task Overlapping_Activites_Update_Exception()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity1 = ActivitySeeds.ActivitySeed();
        var activity2 = ActivitySeeds.ActivitySeed();

        activity1.DateTimeFrom = DateTime.Parse("15/5/2021 18:00");
        activity1.DateTimeTo = DateTime.Parse("15/5/2021 20:00");

        activity2.DateTimeFrom = DateTime.Parse("15/5/2021 20:30");
        activity2.DateTimeTo = DateTime.Parse("15/5/2021 22:00");

        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedActivity1 = await _activityFacade.SaveAsync(activity1, returnedUser.Id, null);
        var returnedActivity2 = await _activityFacade.SaveAsync(activity2, returnedUser.Id, null);

        var DbActivity1 = await _activityFacade.GetAsync(returnedActivity1.Id);

        DbActivity1.DateTimeTo = DateTime.Parse("15/5/2021 21:00");

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

        activity1.DateTimeFrom = DateTime.Parse("15/5/2021 18:00");
        activity1.DateTimeTo = DateTime.Parse("15/5/2021 20:00");

        activity2.DateTimeFrom = DateTime.Parse("15/5/2021 19:00");
        activity2.DateTimeTo = DateTime.Parse("15/5/2021 21:00");


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

