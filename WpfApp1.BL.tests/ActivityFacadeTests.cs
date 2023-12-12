using Microsoft.IdentityModel.Tokens;
using WpfApp1.BL.Facades;
using WpfApp1.BL.Facades.Interfaces;
using WpfApp1.BL.Models;
using WpfApp1.BL.tests.ModelSeeds;
using WpfApp1.DAL.Tests;
using WpfApp1.BL.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace WpfApp1.BL.tests;

public class ActivityFacadeTests : FacadeTestsBase
{
    private readonly UserFacade _userFacade;
    private readonly ActivityFacade _activityFacade;

    public ActivityFacadeTests(ITestOutputHelper output) : base(output)
    {
        _userFacade = new UserFacade(UnitOfWorkFactory, UserModelMapper);
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
        var returnedActivity = await _activityFacade.SaveAsync(activity, returnedUser.Id);

        var DbActivity = await _activityFacade.GetAsync(returnedActivity.Id);

        // Assert
        Assert.NotNull(DbActivity);
        Assert.NotNull(DbActivity.Tags);
        Assert.True(DbActivity.Tags.IsNullOrEmpty());
        DeepAssert.Equal(returnedActivity, DbActivity);
    }


    [Fact]
    public async Task DeleteAcitivty()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity = ActivitySeeds.ActivitySeed();

        // Act
        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedActivity = await _activityFacade.SaveAsync(activity, returnedUser.Id);

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
    public async Task DeleteUserWithAcitivty()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity = ActivitySeeds.ActivitySeed();

        // Act
        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedActivity = await _activityFacade.SaveAsync(activity, returnedUser.Id);

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
    public async Task GetList_OneActivity_DeepAssert()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity = ActivitySeeds.ActivitySeed();

        // Act
        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedActivity = await _activityFacade.SaveAsync(activity, returnedUser.Id);

        // Check
        var DbActivity = await _activityFacade.GetAsync(returnedActivity.Id);
        Assert.NotNull(DbActivity);

        var _activityListModel = new ActivityListModel()
        {
            Id = DbActivity.Id,
            Name = DbActivity.Name,
            DateTimeFrom = DbActivity.DateTimeFrom,
            DateTimeTo = DbActivity.DateTimeTo,
            Color = DbActivity.Color,
            Tags = DbActivity.Tags,
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
        var returnedActivity1 = await _activityFacade.SaveAsync(activity1, returnedUser.Id);
        var returnedActivity2 = await _activityFacade.SaveAsync(activity2, returnedUser.Id);
        var returnedActivity3 = await _activityFacade.SaveAsync(activity3, returnedUser.Id);

        var DbActivity1 = await _activityFacade.GetAsync(returnedActivity1.Id);
        var DbActivity2 = await _activityFacade.GetAsync(returnedActivity2.Id);
        var DbActivity3 = await _activityFacade.GetAsync(returnedActivity3.Id);


        var activityList = await _activityFacade.GetAsyncUser(returnedUser.Id);

        Assert.True(activityList.Any());

        List<Guid> Guids = new ();
        foreach (var activity in activityList)
        {
            Guids.Add(activity.Id);
        }

        Assert.NotNull(DbActivity1);
        Assert.NotNull(DbActivity2);
        Assert.NotNull(DbActivity3);

        Assert.Equal(3, activityList.Count());
        Assert.Contains(DbActivity1.Id, Guids);
        Assert.Contains(DbActivity2.Id, Guids);
        Assert.Contains(DbActivity3.Id, Guids);
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
        var usr1Act1 = await _activityFacade.SaveAsync(activity1, returnedUser1.Id);
        var usr1Act2 = await _activityFacade.SaveAsync(activity2, returnedUser1.Id);
        var usr2Act1 = await _activityFacade.SaveAsync(activity1, returnedUser2.Id);
        var usr2Act2 = await _activityFacade.SaveAsync(activity2, returnedUser2.Id);

        var DbActivity1 = await _activityFacade.GetAsync(usr1Act1.Id);
        var DbActivity2 = await _activityFacade.GetAsync(usr1Act2.Id);
        var DbActivity3 = await _activityFacade.GetAsync(usr2Act1.Id);
        var DbActivity4 = await _activityFacade.GetAsync(usr2Act2.Id);


        var activityList1 = await _activityFacade.GetAsyncUser(returnedUser1.Id);
        var activityList2 = await _activityFacade.GetAsyncUser(returnedUser2.Id);

        Assert.True(activityList1.Any());
        Assert.True(activityList2.Any());

        List<Guid> Guids1 = new ();
        List<Guid> Guids2 = new ();
        foreach (var activity in activityList1)
        {
            Guids1.Add(activity.Id);
        }
        foreach (var activity in activityList2)
        {
            Guids2.Add(activity.Id);
        }

        Assert.NotNull(DbActivity1);
        Assert.NotNull(DbActivity2);
        Assert.NotNull(DbActivity3);
        Assert.NotNull(DbActivity4);

        Assert.Equal(2, activityList1.Count());
        Assert.Contains(DbActivity1.Id, Guids1);
        Assert.Contains(DbActivity2.Id, Guids1);

        Assert.Equal(2, activityList2.Count());
        Assert.Contains(DbActivity3.Id, Guids2);
        Assert.Contains(DbActivity4.Id, Guids2);
    }

    [Fact]
    public async Task DateFilter_NotSupported()
    {
        // Arrange
        var user = UserSeeds.UserSeed();

        var returnedUser = await _userFacade.SaveAsync(user);

        await Assert.ThrowsAsync<NotSupportedException>(async () => await _activityFacade.GetAsyncDateFilter(returnedUser.Id, null, null));
    }


    [Fact]
    public async Task DateFilter_from()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity1 = ActivitySeeds.ActivitySeed();
        var activity2 = ActivitySeeds.ActivitySeed();

        activity1.DateTimeFrom = new DateTime(2021, 05, 15, 18, 00, 00);
        activity1.DateTimeTo = new DateTime(2021, 05, 15, 20, 00, 00);

        activity2.DateTimeFrom = new DateTime(2021, 05, 16, 20, 30, 00);
        activity2.DateTimeTo = new DateTime(2021, 05, 16, 22, 00, 00);

        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedActivity1 = await _activityFacade.SaveAsync(activity1, returnedUser.Id);
        var returnedActivity2 = await _activityFacade.SaveAsync(activity2, returnedUser.Id);


        // Asserts
        var from = new DateTime(2021, 05, 17, 00, 00, 00);
        var list = await _activityFacade.GetAsyncDateFilter(returnedUser.Id, from, null);
        Assert.False(list.Any());


        from = new DateTime(2021, 05, 16, 00, 00, 00);
        list = await _activityFacade.GetAsyncDateFilter(returnedUser.Id, from, null);
        Assert.True(list.Any());
        Assert.True(list.Count() == 1);
        Assert.Equal(returnedActivity2.Id, list.First().Id);


        from = new DateTime(2021, 05, 15, 00, 00, 00);
        list = await _activityFacade.GetAsyncDateFilter(returnedUser.Id, from, null);
        Assert.True(list.Any());
        Assert.True(list.Count() == 2);
    }


    [Fact]
    public async Task DateFilter_to()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity1 = ActivitySeeds.ActivitySeed();
        var activity2 = ActivitySeeds.ActivitySeed();

        activity1.DateTimeFrom = new DateTime(2021, 05, 15, 18, 00, 00);
        activity1.DateTimeTo = new DateTime(2021, 05, 15, 20, 00, 00);

        activity2.DateTimeFrom = new DateTime(2021, 05, 16, 20, 30, 00);
        activity2.DateTimeTo = new DateTime(2021, 05, 16, 22, 00, 00);

        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedActivity1 = await _activityFacade.SaveAsync(activity1, returnedUser.Id);
        var returnedActivity2 = await _activityFacade.SaveAsync(activity2, returnedUser.Id);

        // Asserts
        var to = new DateTime(2021, 05, 15, 00, 00, 00);
        var list = await _activityFacade.GetAsyncDateFilter(returnedUser.Id, null, to);
        Assert.False(list.Any());


        to = new DateTime(2021, 05, 16, 00, 00, 00);
        list = await _activityFacade.GetAsyncDateFilter(returnedUser.Id, null, to);
        Assert.True(list.Any());
        Assert.True(list.Count() == 1);
        Assert.Equal(returnedActivity1.Id, list.First().Id);


        to = new DateTime(2021, 05, 17, 00, 00, 00);
        list = await _activityFacade.GetAsyncDateFilter(returnedUser.Id, null, to);
        Assert.True(list.Any());
        Assert.True(list.Count() == 2);
    }


    [Fact]
    public async Task DateFilter_from_to()
    {
        // Arrange
        var user = UserSeeds.UserSeed();
        var activity1 = ActivitySeeds.ActivitySeed();
        var activity2 = ActivitySeeds.ActivitySeed();
        var activity3 = ActivitySeeds.ActivitySeed();

        activity1.DateTimeFrom = new DateTime(2021, 05, 15, 18, 00, 00);
        activity1.DateTimeTo = new DateTime(2021, 05, 15, 20, 00, 00);

        activity2.DateTimeFrom = new DateTime(2021, 05, 16, 20, 30, 00);
        activity2.DateTimeTo = new DateTime(2021, 05, 16, 22, 00, 00);

        activity3.DateTimeFrom = new DateTime(2021, 05, 17, 20, 30, 00);
        activity3.DateTimeTo = new DateTime(2021, 05, 17, 22, 00, 00);

        var returnedUser = await _userFacade.SaveAsync(user);
        var returnedActivity1 = await _activityFacade.SaveAsync(activity1, returnedUser.Id);
        var returnedActivity2 = await _activityFacade.SaveAsync(activity2, returnedUser.Id);
        var returnedActivity3 = await _activityFacade.SaveAsync(activity3, returnedUser.Id);

        // Asserts
        var from = new DateTime(2021, 05, 16, 00, 00, 00);
        var to = new DateTime(2021, 05, 17, 00, 00, 00);
        var list = await _activityFacade.GetAsyncDateFilter(returnedUser.Id, from, to);
        Assert.True(list.Any());
        Assert.True(list.Count() == 1);
        Assert.Equal(returnedActivity2.Id, list.First().Id);


        from = new DateTime(2021, 05, 15, 00, 00, 00);
        to = new DateTime(2021, 05, 18, 00, 00, 00);
        list = await _activityFacade.GetAsyncDateFilter(returnedUser.Id, from, to);
        Assert.True(list.Any());
        Assert.True(list.Count() == 3);
    }


    [Fact]
    public async Task DateFilter_NoActivities()
    {
        // Arrange
        var user = UserSeeds.UserSeed();

        var returnedUser = await _userFacade.SaveAsync(user);

        var from = new DateTime(2021, 05, 16, 00, 00, 00);
        var to = new DateTime(2021, 05, 17, 00, 00, 00);

        var list = await _activityFacade.GetAsyncDateFilter(returnedUser.Id, from, to);
        Assert.NotNull(list);
        Assert.True(list.IsNullOrEmpty());

        list = await _activityFacade.GetAsyncDateFilter(returnedUser.Id, from, null);
        Assert.NotNull(list);
        Assert.True(list.IsNullOrEmpty());

        list = await _activityFacade.GetAsyncDateFilter(returnedUser.Id, null, to);
        Assert.NotNull(list);
        Assert.True(list.IsNullOrEmpty());
    }


    [Fact]
    public async Task DateFilter_From_MoreUsers()
    {
        // Arrange
        var user1 = UserSeeds.UserSeed();
        var user2 = UserSeeds.UserSeed();
        var activity1 = ActivitySeeds.ActivitySeed();

        activity1.DateTimeFrom = new DateTime(2021, 05, 15, 18, 00, 00);
        activity1.DateTimeTo = new DateTime(2021, 05, 15, 20, 00, 00);

        var returnedUser1 = await _userFacade.SaveAsync(user1);
        var returnedUser2 = await _userFacade.SaveAsync(user2);
        var returnedActivity1 = await _activityFacade.SaveAsync(activity1, returnedUser1.Id);
        var returnedActivity2 = await _activityFacade.SaveAsync(activity1, returnedUser2.Id);


        // Asserts
        var from = new DateTime(2021, 05, 15, 00, 00, 00);
        var list = await _activityFacade.GetAsyncDateFilter(returnedUser1.Id, from, null);
        Assert.True(list.Any());
        Assert.True(list.Count() == 1);

        list = await _activityFacade.GetAsyncDateFilter(returnedUser2.Id, from, null);
        Assert.True(list.Any());
        Assert.True(list.Count() == 1);
    }
}

