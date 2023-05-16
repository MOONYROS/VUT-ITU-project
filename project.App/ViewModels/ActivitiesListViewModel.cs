using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using project.App.Messages;
using project.App.Services.Interfaces;
using project.BL.Facades.Interfaces;
using project.BL.Models;
using System.Collections.ObjectModel;

namespace project.App.ViewModels;

[QueryProperty(nameof(UserId), nameof(UserId))]
public partial class ActivitiesListViewModel : ViewModelBase, 
    IRecipient<ActivityAddMessage>
{
    private readonly IActivityFacade _activityFacade;
    private readonly INavigationService _navigationService;
    public ObservableCollection<ActivityListModel> Activities { get; set; } = null;
    public Guid UserId { get; set; }

    public ActivitiesListViewModel(
        IMessengerService messengerService,
        IActivityFacade activityFacade,
        INavigationService navigationService
        ) : base(messengerService)
    {
        _activityFacade = activityFacade;
        _navigationService = navigationService;
    }

    protected override async Task LoadDataAsync()
    {
        var act = await _activityFacade.GetAsyncUser(UserId);
        Activities = act.ToObservableCollection();
    }

    [RelayCommand]
    private async void GoToAddActivity()
    {
        await _navigationService.GoToAsync<AddActivityViewModel>(
                new Dictionary<string, object?> { [nameof(AddActivityViewModel.UserId)] = UserId });
    }

    public async void Receive(ActivityDeleteMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(ActivityAddMessage message)
    {
        await LoadDataAsync();
    }
}
