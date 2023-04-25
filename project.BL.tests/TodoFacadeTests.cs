using project.BL.Facades;
using project.BL.Facades.Interfaces;
using project.BL.Models;
using project.DAL.Tests;

namespace project.BL.tests;

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
    public async Task TodoSaveAsyncTest()
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

    [Fact]
    public async Task TodoGetAsyncTest()
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
        var returnedTodoModel = await _todoFacadeSUT.SaveAsync(todo, dbUserModel.Id);
        todo.Id = returnedTodoModel.Id;


        var todoDbModel = await _todoFacadeSUT.GetAsync(todo.Id);
        DeepAssert.Equal(todoDbModel, todo);
    }

    [Fact]
    public async Task TodoDeleteAsyncTest()
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
        var returnedTodoModel = await _todoFacadeSUT.SaveAsync(todo, dbUserModel.Id);
        todo.Id = returnedTodoModel.Id;


        var todoDbModel = await _todoFacadeSUT.GetAsync(todo.Id);
        DeepAssert.Equal(todoDbModel, todo);

        await _todoFacadeSUT.DeleteAsync(todo.Id);

        var expectedNull = await _userFacadeSUT.GetAsync(todo.Id);
        Assert.Null(expectedNull);
    }
}