using project.App.Models;
using project.App.Services.Interfaces;
using project.App.ViewModels;
using project.App.Views;

namespace project.App.Services;

public class NavigationService : INavigationService
{
    public IEnumerable<RouteModel> Routes { get; } = new List<RouteModel>
    { 
        new("main" , typeof(MainView), typeof(MainViewModel)),
        new("main/newUser", typeof(AddUserView), typeof(AddUserViewModel)),
        new("main/activities", typeof(ActivitiesView), typeof(ActivitiesViewModel)),
        new("main/activities/userActivities", typeof(ActivitiesListView), typeof(ActivitiesListViewModel)),
        new("main/activities/userActivities/addActivity", typeof(AddActivityView), typeof(AddActivityViewModel)),
        new("main/activities/userTodos", typeof(TodoListView), typeof(TodoListViewModel)),
        new("main/activities/userTodos/addTodo", typeof(AddTodoView), typeof(AddTodoViewModel)),
        new("main/activities/userProject", typeof(ProjectListView), typeof(ProjectListViewModel))
    };

    public async Task GoToAsync<TViewModel>()
        where TViewModel : IViewModel
    {
        var route = GetRouteByViewModel<TViewModel>();
        await Shell.Current.GoToAsync(route);
    }
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    public async Task GoToAsync<TViewModel>(IDictionary<string, object?> parameters)
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        where TViewModel : IViewModel
    {
        var route = GetRouteByViewModel<TViewModel>();
        await Shell.Current.GoToAsync(route, parameters);
    }

    public async Task GoToAsync(string route)
        => await Shell.Current.GoToAsync(route);

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    public async Task GoToAsync(string route, IDictionary<string, object?> parameters)
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        => await Shell.Current.GoToAsync(route, parameters);

    public bool SendBackButtonPressed()
        => Shell.Current.SendBackButtonPressed();

    private string GetRouteByViewModel<TViewModel>()
        where TViewModel : IViewModel
        => Routes.First(route => route.ViewModelType == typeof(TViewModel)).Route;
}
