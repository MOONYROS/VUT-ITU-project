using project.BL.Facades.Interfaces;
using project.BL.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using project.BL.Models;
using System.Drawing;

namespace project.BL.tests
{
    public class TagFacadeTests : FacadeTestsBase
    {
        private readonly ITagFacade _tagFacadeSUT;

        public TagFacadeTests()
        {
            _tagFacadeSUT = new TagFacade(UnitOfWorkFactory, TagModelMapper);
        }

        [Fact]
        public async Task TagTest1()
        {
            var model = new TagDetailModel()
            {
                Id = Guid.Empty,
                Name = "Test",
                Color = Color.Black
            };

            var _ = await _tagFacadeSUT.SaveAsync(model);
        }


        [Fact]
        public async Task TagTest2()
        {

        }

    }
}
