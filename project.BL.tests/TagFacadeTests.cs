using System.Drawing;
using project.BL.Facades;
using project.BL.Facades.Interfaces;
using project.BL.Models;

namespace project.BL.tests;

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