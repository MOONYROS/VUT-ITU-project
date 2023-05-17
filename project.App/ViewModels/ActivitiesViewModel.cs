using CommunityToolkit.Mvvm.Input;
using project.App.Services.Interfaces;

namespace project.App.ViewModels;

[QueryProperty(nameof(UserId), nameof(UserId))]
public partial class ActivitiesViewModel : ViewModelBase
{
    private INavigationService _navigationService;
    public Guid UserId { get; set; }

    public ActivitiesViewModel(IMessengerService messengerService,
        INavigationService navigationService)
        :base(messengerService)
    {
        _navigationService = navigationService;
    }

    [RelayCommand]
    private async void GoToActivitiesList()
    {
        await _navigationService.GoToAsync<ActivitiesListViewModel>(
                new Dictionary<string, object?> { [nameof(ActivitiesListViewModel.UserId)] = UserId });
    }

    [RelayCommand]
    private async void GoToTodoList()
    {
        await _navigationService.GoToAsync<TodoListViewModel>(
                new Dictionary<string, object?> { [nameof(TodoListViewModel.UserId)] = UserId });
    }
    
    [RelayCommand]
    private async void GoToProjectList()
    {
        await _navigationService.GoToAsync<ProjectListViewModel>(
                new Dictionary<string, object?> { [nameof(ProjectListViewModel.UserId)] = UserId });
    }
}
