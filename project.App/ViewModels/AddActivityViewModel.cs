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
    public ActivityDetailModel activityDetailModel { get; set; } = ActivityDetailModel.Empty;
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
        if (TimeFrom < TimeTo && activityDetailModel.Name != string.Empty)
        {
            activityDetailModel.DateTimeFrom = activityDetailModel.DateTimeFrom + TimeFrom;
            activityDetailModel.DateTimeTo = activityDetailModel.DateTimeTo + TimeTo;
            activityDetailModel.Color = IndexToColor(ColorIndex);
            try
            {
                await _activityFacade.SaveAsync(activityDetailModel, UserId, null);
            }
            catch (OverlappingException)
            {
                await _alertService.DisplayAsync("Hupsik Dupsik", "Activites are overlapping");
            }
            messengerService.Send(new ActivityAddMessage());
            activityDetailModel = ActivityDetailModel.Empty;
        }
    }
    private System.Drawing.Color IndexToColor(int index)
    {
        return System.Drawing.Color.Green;
    }
}


