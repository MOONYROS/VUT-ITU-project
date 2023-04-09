using project.BL.Facades;
using project.BL.Facades.Interfaces;
using project.DAL.Tests;

namespace project.BL.tests
{
    public class UserFacadeTests :  FacadeTestsBase
    {
        private readonly IUserFacade _userFacadeSUT;

        public UserFacadeTests()
        {
            _userFacadeSUT = new UserFacade(UnitOfWorkFactory, UserModelMapper);
        }

        [Fact]
        public async Task UserFacadeSaveAsyncTest()
        {
            var userModel = ModelSeeds.UserSeeds.UserSeed();
            var returnedDbModel = await _userFacadeSUT.SaveAsync(userModel);
            userModel.Id = returnedDbModel.Id;

            DeepAssert.Equal(userModel, returnedDbModel);
        }

        [Fact]
        public async Task UserFacadeGetAsyncTest()
        {
            var userModel = ModelSeeds.UserSeeds.UserSeed();
            var returnedDbModel = await _userFacadeSUT.SaveAsync(userModel);
            userModel.Id = returnedDbModel.Id;

            var userDbModel = await _userFacadeSUT.GetAsync(userModel.Id);
            DeepAssert.Equal(userModel, userDbModel);
        }

        [Fact]
        public async Task UserFacadeDeleteAsyncTest()
        {
            var userModel = ModelSeeds.UserSeeds.UserSeed();
            var returnedDbModel = await _userFacadeSUT.SaveAsync(userModel);
            userModel.Id = returnedDbModel.Id;
            
            var userDbModel = await _userFacadeSUT.GetAsync(userModel.Id);
            DeepAssert.Equal(userModel, userDbModel);

            await _userFacadeSUT.DeleteAsync(userModel.Id);

            var expectedNull = await _userFacadeSUT.GetAsync(userModel.Id);
            Assert.Null(expectedNull);
        }
    }
}
