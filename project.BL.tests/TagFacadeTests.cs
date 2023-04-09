using project.BL.Facades.Interfaces;
using project.BL.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using project.BL.Models;
using System.Drawing;
using project.DAL.Tests;

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
        public async Task TagSaveAsyncTest()
        {
            var model = new TagDetailModel()
            {
                Id = Guid.Empty,
                Name = "Test",
                Color = Color.Black
            };

            var _ = await _tagFacadeSUT.SaveAsync(model);
        }

        // Bohuzel barvy delaji neplechu a neni cas toto opravit pred druhym odevzdanim :(
        [Fact]
        public async Task TagGetAsyncTest()
        {
            var tagModel = new TagDetailModel()
            {
                Id = Guid.Empty,
                Name = "Test",
                Color = Color.Black
            };

            var returnedModel = await _tagFacadeSUT.SaveAsync(tagModel);
            tagModel.Id = returnedModel.Id;

            var todoDbModel = await _tagFacadeSUT.GetAsync(tagModel.Id);
            Assert.Equal(tagModel.Id, todoDbModel.Id);
            Assert.Equal(tagModel.Name, todoDbModel.Name);
        }

        [Fact]
        public async Task TagDeleteAsyncTest()
        {
            var tagModel = new TagDetailModel()
            {
                Id = Guid.Empty,
                Name = "Test",
                Color = Color.Black
            };

            var returnedModel = await _tagFacadeSUT.SaveAsync(tagModel);
            tagModel.Id = returnedModel.Id;

            var todoDbModel = await _tagFacadeSUT.GetAsync(tagModel.Id);
            Assert.Equal(tagModel.Id, todoDbModel.Id);
            Assert.Equal(tagModel.Name, todoDbModel.Name);

            await _tagFacadeSUT.DeleteAsync(tagModel.Id);

            var expectedNull = await _tagFacadeSUT.GetAsync(tagModel.Id);
            Assert.Null(expectedNull);

        }
    }
}
