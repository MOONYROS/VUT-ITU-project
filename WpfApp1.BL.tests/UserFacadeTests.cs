using WpfApp1.BL.Facades;
using WpfApp1.BL.Models;
using WpfApp1.BL.tests.ModelSeeds;
using Xunit.Abstractions;
using WpfApp1.DAL.Tests;

namespace WpfApp1.BL.tests;

public class UserFacadeTests : FacadeTestsBase
{
    private readonly UserFacade _userFacade;
    public UserFacadeTests(ITestOutputHelper output) : base(output)
    {
        _userFacade = new UserFacade(UnitOfWorkFactory, UserModelMapper);
    }

    [Fact]
    public async Task CreateUser_Succeeds()
    {
        // Arrange
        var userModel = UserSeeds.UserSeed();
        
        // Act
        var idk = await _userFacade.SaveAsync(userModel);
        var idk2 = await _userFacade.GetAsync(idk.Id);
        
        // Assert
        DeepAssert.Equal(idk2, idk);
    }

    [Fact]
    public async Task TwoModels_InsertFirst_SearchSecond_IsNull()
    {
        // Arrange
        var user1 = UserSeeds.UserSeed();
        var user2 = UserSeeds.UserSeed();
        
        // Act
        var user1Updated = await _userFacade.SaveAsync(user1);
        var userFromDb = await _userFacade.GetAsync(user2.Id);
        
        // Assert
        Assert.Null(userFromDb);
    }

    [Fact]
    public async Task InsertFive_FindsCorrect()
    {
        // Arrange
        var user1 = UserSeeds.UserSeed();
        var user2 = UserSeeds.UserSeed();
        var user3 = UserSeeds.UserSeed();
        var user4 = UserSeeds.UserSeed();
        var user5 = UserSeeds.UserSeed();
        
        // Act
        var user1Updated = await _userFacade.SaveAsync(user1);
        var user2Updated = await _userFacade.SaveAsync(user2);
        var user3Updated = await _userFacade.SaveAsync(user3);
        var user4Updated = await _userFacade.SaveAsync(user4);
        var user5Updated = await _userFacade.SaveAsync(user5);

        var userFromDb = await _userFacade.GetAsync(user4Updated.Id);
        
        // Assert
        Assert.NotNull(userFromDb);
        DeepAssert.Equal(user4Updated, userFromDb);
    }

    [Fact]
    public async Task DeleteUser_Success()
    {
        // Arrange
        var userModel = UserSeeds.UserSeed();
        
        // Act
        var returnedModel = await _userFacade.SaveAsync(userModel);
        await _userFacade.DeleteAsync(returnedModel.Id);
        
        // Assert
        var shouldBeNull = await _userFacade.GetAsync(returnedModel.Id);
        Assert.Null(shouldBeNull);
    }

    [Fact]
    public async Task DeleteNonExistingUser_Exception()
    {
        // Arrange
        var userModel = UserSeeds.UserSeed();

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _userFacade.DeleteAsync(userModel.Id));
    }


    [Fact]
    public async Task InsertMoreUsers_DeleteCorrect()
    {
        // Arrange
        var user1 = UserSeeds.UserSeed();
        var user2 = UserSeeds.UserSeed();
        var user3 = UserSeeds.UserSeed();
        var user4 = UserSeeds.UserSeed();
        var user5 = UserSeeds.UserSeed();

        // Act
        var user1Updated = await _userFacade.SaveAsync(user1);
        var user2Updated = await _userFacade.SaveAsync(user2);
        var user3Updated = await _userFacade.SaveAsync(user3);
        var user4Updated = await _userFacade.SaveAsync(user4);
        var user5Updated = await _userFacade.SaveAsync(user5);

        await _userFacade.DeleteAsync(user2Updated.Id);
        var shouldBeNull = await _userFacade.GetAsync(user2Updated.Id);
        
        // Assert
        Assert.Null(shouldBeNull);
    }


    [Fact]
    public async Task UpdateUser_Correct()
    {
        // Arrange
        var userModel = UserSeeds.UserSeed();

        // Act
        var insertedModel = await _userFacade.SaveAsync(userModel);
        var dbModel = await _userFacade.GetAsync(insertedModel.Id);

        Assert.NotNull(dbModel);
        
        dbModel.FullName = "Ales Bejr";
        await _userFacade.SaveAsync(dbModel);

        var dbModelUpdated = await _userFacade.GetAsync(insertedModel.Id);

        DeepAssert.Equal(dbModel, dbModelUpdated);
    }


    [Fact]
    public async Task GetListModels_Correct()
    {
        // Arrange
        var user1 = UserSeeds.UserSeed();
        var user2 = UserSeeds.UserSeed();
        var user3 = UserSeeds.UserSeed();

        // Act
        var user1Updated = await _userFacade.SaveAsync(user1);
        var user2Updated = await _userFacade.SaveAsync(user2);
        var user3Updated = await _userFacade.SaveAsync(user3);

        var UserList = await _userFacade.GetAsync();

        var user1Listmodel = new UserListModel()
        {
            Id = user1Updated.Id,
            UserName = user1Updated.UserName
        };

        var user2Listmodel = new UserListModel()
        {
            Id = user2Updated.Id,
            UserName = user2Updated.UserName
        };

        var user3Listmodel = new UserListModel()
        {
            Id = user3Updated.Id,
            UserName = user3Updated.UserName
        };

        var userListNotInDb = new UserListModel()
        {
            Id = Guid.NewGuid(),
            UserName = "Ales Bejr"
        };

        Assert.Contains(user1Listmodel, UserList);
        Assert.Contains(user2Listmodel, UserList);
        Assert.Contains(user3Listmodel, UserList);

        Assert.DoesNotContain(userListNotInDb, UserList);
    }


    [Fact]
    public async Task GetListModels_Empty()
    {
        var UserList = await _userFacade.GetAsync();

        Assert.Empty(UserList);
    }
}