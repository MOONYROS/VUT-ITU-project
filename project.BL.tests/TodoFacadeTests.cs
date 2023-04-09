using project.BL.Facades.Interfaces;
using project.BL.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using project.BL.Models;

namespace project.BL.tests
{
    public class TodoFacadeTests : FacadeTestsBase
    {
        private readonly ITodoFacade _todoFacadeSUT;

        public TodoFacadeTests()
        {
            _todoFacadeSUT = new TodoFacade(UnitOfWorkFactory, TodoModelMapper);
        }

        [Fact]
        public async Task TodoTest()
        {
            var user = ModelSeeds.UserSeeds.UserSeed();

            var model = new TodoDetailModel()
            {
                Id = Guid.Empty,
                Name = "Test",
                Finished = false,
                Date = DateOnly.Parse("January 1, 2000")
            };

            var _ = await _todoFacadeSUT.SaveAsync(model, user.);
        }

    }
}
