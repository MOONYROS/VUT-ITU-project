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
    private readonly TagFacade _tagFacade;
    private readonly ActivityTagFacade _activityTagFacade;

    public ActivityFacadeTests(ITestOutputHelper output) : base(output)
    {
        _userFacade = new UserFacade(UnitOfWorkFactory, UserModelMapper);
        _activityFacade = new ActivityFacade(UnitOfWorkFactory, ActivityModelMapper);
        _tagFacade = new TagFacade(UnitOfWorkFactory, TagModelMapper);
        _activityTagFacade = new ActivityTagFacade(UnitOfWorkFactory);
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
	    
	    // empty list
	    IEnumerable<Guid> userIds = new List<Guid>();

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

    [Fact]
    public async Task AddTwoUsersOneActivity_DeleteUser_ActivityPersists()
    {
	    var activity = ActivitySeeds.ActivitySeed();
	    var user = UserSeeds.UserSeed();
	    var user2 = UserSeeds.UserSeed();

	    var returnedUser = await _userFacade.SaveAsync(user);
	    var returnedUser2 = await _userFacade.SaveAsync(user2);
	    IEnumerable<Guid> userIds = new List<Guid>
	    {
		    returnedUser.Id,
		    returnedUser2.Id
	    };
	    var returnedActivity = await _activityFacade.CreateActivityAsync(activity, userIds);

	    await _userFacade.DeleteAsync(returnedUser.Id);

	    var dbActivity = await _activityFacade.GetAsync(returnedActivity.Id);

	    Assert.NotNull(dbActivity);
	    Assert.Equal(dbActivity.Users.First().Id, returnedUser2.Id);
    }
    
    [Fact]
    public async Task Datefilter_from()
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
	    
	    IEnumerable<Guid> userIds = new List<Guid>
	    {
		    returnedUser.Id,
	    };

	    var returnedActivity1 = await _activityFacade.CreateActivityAsync(activity1, userIds);
	    var returnedActivity2 = await _activityFacade.CreateActivityAsync(activity2, userIds);


	    // Asserts
	    var from = new DateTime(2021, 05, 17, 00, 00, 00);
	    var list = await _activityFacade.GetActivitiesDateFilterAsync(returnedUser.Id, from, null);
	    Assert.False(list.Any());


	    from = new DateTime(2021, 05, 16, 00, 00, 00);
	    list = await _activityFacade.GetActivitiesDateFilterAsync(returnedUser.Id, from, null);
	    Assert.True(list.Any());
	    Assert.True(list.Count() == 1);
	    Assert.Equal(returnedActivity2.Id, list.First().Id);


	    from = new DateTime(2021, 05, 15, 00, 00, 00);
	    list = await _activityFacade.GetActivitiesDateFilterAsync(returnedUser.Id, from, null);
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
	    
	    IEnumerable<Guid> userIds = new List<Guid>
	    {
		    returnedUser.Id,
	    };

	    var returnedActivity1 = await _activityFacade.CreateActivityAsync(activity1, userIds);
	    var returnedActivity2 = await _activityFacade.CreateActivityAsync(activity2, userIds);


	    // Asserts
	    var to = new DateTime(2021, 05, 15, 00, 00, 00);
	    var list = await _activityFacade.GetActivitiesDateFilterAsync(returnedUser.Id, null, to);
	    Assert.False(list.Any());


	    to = new DateTime(2021, 05, 16, 00, 00, 00);
	    list = await _activityFacade.GetActivitiesDateFilterAsync(returnedUser.Id, null, to);
	    Assert.True(list.Any());
	    Assert.True(list.Count() == 1);
	    Assert.Equal(returnedActivity1.Id, list.First().Id);


	    to = new DateTime(2021, 05, 17, 00, 00, 00);
	    list = await _activityFacade.GetActivitiesDateFilterAsync(returnedUser.Id, null, to);
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

	    activity1.DateTimeFrom = new DateTime(2021, 05, 15, 18, 00, 00);
	    activity1.DateTimeTo = new DateTime(2021, 05, 15, 20, 00, 00);

	    activity2.DateTimeFrom = new DateTime(2021, 05, 16, 20, 30, 00);
	    activity2.DateTimeTo = new DateTime(2021, 05, 16, 22, 00, 00);
	    
	    var returnedUser = await _userFacade.SaveAsync(user);
	    
	    IEnumerable<Guid> userIds = new List<Guid>
	    {
		    returnedUser.Id,
	    };

	    var returnedActivity1 = await _activityFacade.CreateActivityAsync(activity1, userIds);
	    var returnedActivity2 = await _activityFacade.CreateActivityAsync(activity2, userIds);


	    // Asserts
	    var from = new DateTime(2021, 05, 14, 00, 00, 00);
	    var to = new DateTime(2021, 05, 15, 00, 00, 00);
	    var list = await _activityFacade.GetActivitiesDateFilterAsync(returnedUser.Id, from, to);
	    Assert.False(list.Any());


	    from = new DateTime(2021, 05, 15, 00, 00, 00);
	    to = new DateTime(2021, 05, 16, 00, 00, 00);
	    list = await _activityFacade.GetActivitiesDateFilterAsync(returnedUser.Id, from, to);
	    Assert.True(list.Any());
	    Assert.True(list.Count() == 1);
	    Assert.Equal(returnedActivity1.Id, list.First().Id);


	    from = new DateTime(2021, 05, 15, 00, 00, 00);
	    to = new DateTime(2021, 05, 17, 00, 00, 00);
	    list = await _activityFacade.GetActivitiesDateFilterAsync(returnedUser.Id, from, to);
	    Assert.True(list.Any());
	    Assert.True(list.Count() == 2);
    }
    
    
    [Fact]
    public async Task TagFilter()
    {
	    var user = UserSeeds.UserSeed();
	    var activity = ActivitySeeds.ActivitySeed();
	    var tag = TagSeeds.TagSeed();
	    var tag2 = TagSeeds.TagSeed();

	    var returnedUser = await _userFacade.SaveAsync(user);
	    IEnumerable<Guid> userIds = new List<Guid>
	    {
		    returnedUser.Id,
	    };
	    var returnedActivity = await _activityFacade.CreateActivityAsync(activity, userIds);

	    var returnedTag = await _tagFacade.SaveAsync(tag, returnedUser.Id);
	    var returnedTag2 = await _tagFacade.SaveAsync(tag2, returnedUser.Id);

	    var list = await _activityFacade.GetActivitiesTagFilterAsync(returnedUser.Id, returnedTag.Id);
	    
	    Assert.Empty(list);

	    await _activityTagFacade.SaveAsync(returnedActivity.Id, returnedTag.Id);
	    
	    list = await _activityFacade.GetActivitiesTagFilterAsync(returnedUser.Id, returnedTag.Id);
		
	    Assert.NotEmpty(list);
	    
	    list = await _activityFacade.GetActivitiesTagFilterAsync(returnedUser.Id, returnedTag2.Id);
	    
	    Assert.Empty(list);
    }
    
    
    [Fact]
    public async Task DateTagFilter()
    {
	    var user = UserSeeds.UserSeed();
	    var activity = ActivitySeeds.ActivitySeed();
	    activity.DateTimeFrom = new DateTime(2021, 05, 15, 18, 00, 00);
	    activity.DateTimeTo = new DateTime(2021, 05, 15, 20, 00, 00);
	    var tag = TagSeeds.TagSeed();

	    var returnedUser = await _userFacade.SaveAsync(user);
	    IEnumerable<Guid> userIds = new List<Guid>
	    {
		    returnedUser.Id,
	    };
	    var returnedActivity = await _activityFacade.CreateActivityAsync(activity, userIds);

	    var returnedTag = await _tagFacade.SaveAsync(tag, returnedUser.Id);
	    
	    var from = new DateTime(2021, 05, 15, 00, 00, 00);
	    var to = new DateTime(2021, 05, 16, 00, 00, 00);

	    var list = await _activityFacade.GetActivitiesDateTagFilterAsync
		    (returnedUser.Id, from, to, returnedTag.Id);
	    
	    Assert.Empty(list);

	    await _activityTagFacade.SaveAsync(returnedActivity.Id, returnedTag.Id);
	    
	    list = await _activityFacade.GetActivitiesDateTagFilterAsync
		   (returnedUser.Id, from, to, returnedTag.Id);
		   
	    Assert.NotEmpty(list);
    }
}

