using project.BL.Facades.Interfaces;
using project.BL.Facades;
using project.BL.Models;

namespace project.BL.tests
{
    public class TodoFacadeTests : FacadeTestsBase
    {
        private readonly ITodoFacade _todoFacadeSUT;
        private readonly UserFacade _userFacadeSUT;

        public TodoFacadeTests()
        {
            _todoFacadeSUT = new TodoFacade(UnitOfWorkFactory, TodoModelMapper);
            _userFacadeSUT = new UserFacade(UnitOfWorkFactory, UserModelMapper);
        }

        [Fact]
        public async Task TodoTest()
        {
            var user = ModelSeeds.UserSeeds.UserSeed();

            var todo = new TodoDetailModel()
            {
                Id = Guid.Empty,
                Name = "Test",
                Finished = false,
                Date = DateOnly.Parse("January 1, 2000")
            };

            var dbUserModel = await _userFacadeSUT.SaveAsync(user);
            var _ = await _todoFacadeSUT.SaveAsync(todo, dbUserModel.Id);
        }
    }
}