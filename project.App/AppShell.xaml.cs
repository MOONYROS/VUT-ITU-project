using CommunityToolkit.Mvvm.Input;
using project.App.Services.Interfaces;

namespace project.App;

public partial class AppShell : Shell
{
    private readonly INavigationService _navigationService;
    public AppShell(INavigationService navigationService)
    {
        _navigationService = navigationService;
        InitializeComponent();
    }

    [RelayCommand]
    private async void GoToActivitiesList()
    {
        await _navigationService.GoToAsync("main/activities/userActivities");
    }
    
    [RelayCommand]
    private async void GoToTodoList()
    {
        await _navigationService.GoToAsync("main/activities/userTodos");
    }
}