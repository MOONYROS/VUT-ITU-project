using CommunityToolkit.Mvvm.Input;
using project.App.Services.Interfaces;

namespace project.App.ViewModels;

[QueryProperty(nameof(UserId), nameof(UserId))]
public partial class ActivitiesViewModel : ViewModelBase
{
    private INavigationService _navigationService;
    public IEnumerable<DateTime> Week { get; set; }
    public DateTime Today { get; set; }
    public Guid UserId { get; set; }   

    public ActivitiesViewModel(IMessengerService messengerService,
        INavigationService navigationService)
        :base(messengerService)
    {
        _navigationService = navigationService;
        Week = new List<DateTime>();
        Today = DateTime.Today;
        while (Today.DayOfWeek != DayOfWeek.Monday)
        {
            Today = Today.AddDays(-1);
        }

        for (int i = 0; i < 7; i++)
        {
            Week = Week.Append(Today);
            Today = Today.AddDays(1);
        }
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
