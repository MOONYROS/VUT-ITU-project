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
    public int SortType { get; set; }

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

    [RelayCommand]

    private async void SortActivities()
    {
        IEnumerable<ActivityListModel> act; 
        switch (SortType)
        {
            case 0:
                act = await _activityFacade.GetAsyncUser(UserId);
                break;
            case 1:
                act = await _activityFacade.GetAsyncDateFilter(UserId, DateTime.Today.AddDays(-7), DateTime.Today);   
                break;
            case 2:
                act = await _activityFacade.GetAsyncDateFilter(UserId, DateTime.Today.AddMonths(-1), DateTime.Today);
                break;
            case 3:
                act = await _activityFacade.GetAsyncDateFilter(UserId, DateTime.Today.AddMonths(-2), DateTime.Today);
                break;
            case 4: 
                act = await _activityFacade.GetAsyncDateFilter(UserId, DateTime.Today.AddYears(-1), DateTime.Today);
                break;
            default: 
                act = await _activityFacade.GetAsyncUser(UserId);
                break;
        }
        
        Activities = act.ToObservableCollection();
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
