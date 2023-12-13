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
    public async Task CreateActivity()
    {
	    var activity = ActivitySeeds.ActivitySeed();
	    var user = UserSeeds.UserSeed();
	    
	    var returnedUser = await _userFacade.SaveAsync(user);
	    
	    IEnumerable<Guid> userIds = new List<Guid>();
	    userIds = userIds.Append(returnedUser.Id);

	    var returnedActivity = await _activityFacade.CreateActivityAsync(activity,userIds);

	    var dbActivities = await _activityFacade.GetUserActivitiesAsync(returnedUser.Id);

	    var dbActivity = await _activityFacade.GetAsync(returnedActivity.Id);

	    Assert.NotNull(dbActivity);
	    Assert.Equal(dbActivity.Users.First().Id, returnedUser.Id);
	    Assert.NotEmpty(dbActivities);
    }
    
    [Fact]
    public async Task CreateActivity_NoUsers()
    {
	    var activity = ActivitySeeds.ActivitySeed();
	    
	    IEnumerable<Guid> userIds = new List<Guid>();// empty list

	    var returnedActivity = await _activityFacade.CreateActivityAsync(activity,userIds);

	    var dbActivity = await _activityFacade.GetAsync(returnedActivity.Id);
	    Assert.NotNull(dbActivity);
    }

    [Fact]
    public async Task RemoveActivity_ActivityDeleted()
    {
	    var activity = ActivitySeeds.ActivitySeed();
	    var user = UserSeeds.UserSeed();
	    
	    var returnedUser = await _userFacade.SaveAsync(user);
	    
	    IEnumerable<Guid> userIds = new List<Guid>();
	    userIds = userIds.Append(returnedUser.Id);

	    var returnedActivity = await _activityFacade.CreateActivityAsync(activity,userIds);
	    var dbActivities = await _activityFacade.GetUserActivitiesAsync(returnedUser.Id);
	    var dbActivity = await _activityFacade.GetAsync(returnedActivity.Id);
	    Assert.NotNull(dbActivity);
	    Assert.Equal(dbActivity.Users.First().Id, returnedUser.Id);
	    Assert.NotEmpty(dbActivities);

	    await _activityFacade.RemoveActivityFromUserAsync(dbActivity.Id, returnedUser.Id);
	    dbActivity = await _activityFacade.GetAsync(returnedActivity.Id);
		Assert.Null(dbActivity);
    }

    [Fact]
    public async Task RemoveActivity_ActivityRemains()
    {
	    var activity = ActivitySeeds.ActivitySeed();
	    var user1 = UserSeeds.UserSeed();
	    var user2 = UserSeeds.UserSeed();
	    
	    var returnedUser1 = await _userFacade.SaveAsync(user1);
	    var returnedUser2 = await _userFacade.SaveAsync(user2);
	    
	    IEnumerable<Guid> userIds = new List<Guid>();
	    userIds = userIds.Append(returnedUser1.Id).Append(returnedUser2.Id);

	    var returnedActivity = await _activityFacade.CreateActivityAsync(activity,userIds);
	    var dbActivity = await _activityFacade.GetAsync(returnedActivity.Id);
	    Assert.NotNull(dbActivity);

	    await _activityFacade.RemoveActivityFromUserAsync(dbActivity.Id, returnedUser1.Id);
	    dbActivity = await _activityFacade.GetAsync(dbActivity.Id);
	    Assert.NotNull(dbActivity);
    }

    [Fact]
    public async Task AddActivityAndUser_DeleteUser_ActivityDeleted()
    {
	    var activity = ActivitySeeds.ActivitySeed();
	    var user = UserSeeds.UserSeed();

	    var returnedUser = await _userFacade.SaveAsync(user);
	    IEnumerable<Guid> userIds = new List<Guid>
	    {
		    returnedUser.Id
	    };
	    var returnedActivity = await _activityFacade.CreateActivityAsync(activity, userIds);

	    await _userFacade.DeleteAsync(returnedUser.Id);

	    var dbActivity = await _activityFacade.GetAsync(returnedActivity.Id);

	    Assert.Null(dbActivity);
    }
}

