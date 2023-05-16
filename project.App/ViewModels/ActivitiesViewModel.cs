using CommunityToolkit.Mvvm.Input;
using project.App.Services.Interfaces;

namespace project.App.ViewModels;

public partial class ActivitiesViewModel : ViewModelBase
{
    private INavigationService _navigationService;
    public IEnumerable<DateTime> Week { get; set; }
    public DateTime Today { get; set; }

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
        await _navigationService.GoToAsync("main/activities/userActivities");
    }

    [RelayCommand]
    private async void GoToTodoList()
    {
        await _navigationService.GoToAsync("main/activities/userTodos");
    }
}
