using project.BL.Facades;
using project.BL.tests.ModelSeeds;
using Xunit.Abstractions;
using Xunit.Asserts.Compare;

namespace project.BL.tests;

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
        var userModel = UserSeeds.UserSeed();
        var idk = await _userFacade.SaveAsync(userModel);
    }

    [Fact]
    public async Task CreateUserModel_GetEntity_CompareID()
    {
        var userModel = UserSeeds.UserSeed();
        
        // Vraci userModel, ale ma novy guid (zmeni se ve fasade)
        var userModelUpdated = await _userFacade.SaveAsync(userModel);
    
        var userFromDb = await _userFacade.GetAsync(userModelUpdated.Id);
        Assert.NotNull(userFromDb);
        Assert.Equal(userModelUpdated.Id, userFromDb.Id);
    }
    
}