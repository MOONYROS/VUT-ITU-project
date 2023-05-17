using CommunityToolkit.Mvvm.Input;
using project.App.Messages;
using project.App.Services.Interfaces;
using project.BL.Facades.Interfaces;
using project.BL.Models;
using System.Drawing;
using project.BL;

namespace project.App.ViewModels;

[QueryProperty(nameof(UserId), nameof(UserId))]
public partial class AddActivityViewModel : ViewModelBase
{
    private readonly IActivityFacade _activityFacade;
    private readonly INavigationService _navigationService;
    private readonly IAlertService _alertService;
    public Guid UserId { get; set; }
    public int ColorIndex { get; set; } = 0;

    public TimeSpan TimeFrom { get; set; }
    public TimeSpan TimeTo { get; set; }
    public ActivityDetailModel ActivityDetailModel { get; set; } = ActivityDetailModel.Empty;
    public AddActivityViewModel(
        IMessengerService messengerService,
        IActivityFacade activityFacade,
        INavigationService navigationService,
        IAlertService alertService)
        : base(messengerService)
    {
        _activityFacade = activityFacade;
        _navigationService = navigationService;
        _alertService = alertService;
    }

    [RelayCommand]
    public async Task SaveActivityAsync()
    {
        ActivityDetailModel.DateTimeFrom += TimeFrom;
        ActivityDetailModel.DateTimeTo += TimeTo;
        if (ActivityDetailModel.Name == string.Empty)
        {
            await _alertService.DisplayAsync("Hupsik Dupsik", "Please enter activity name");
        }
        else if (ActivityDetailModel.DateTimeFrom > ActivityDetailModel.DateTimeTo)
        {
            await _alertService.DisplayAsync("Hupsik Dupsik", "Activity ends before it starts");
        }
        else
        {
            
            ActivityDetailModel.Color = IndexToColor(ColorIndex);
            try
            {
                await _activityFacade.SaveAsync(ActivityDetailModel, UserId, null);
            }
            catch (OverlappingException)
            {
                await _alertService.DisplayAsync("Hupsik Dupsik", "Activites are overlapping");
            }
            messengerService.Send(new ActivityAddMessage());
            ActivityDetailModel = ActivityDetailModel.Empty;
            await _navigationService.GoToAsync<ActivitiesListViewModel>(
                new Dictionary<string, object?> { [nameof(ActivitiesListViewModel.UserId)] = UserId });
        }
    }
    private System.Drawing.Color IndexToColor(int index)
    {
        return System.Drawing.Color.Green;
    }
}


