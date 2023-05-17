using System.Diagnostics;
using project.App.Services.Interfaces;
using project.BL.Facades.Interfaces;
using project.BL.Models;

namespace project.App.ViewModels;

[QueryProperty(nameof(ActivityId), nameof(ActivityId))]
public partial class ActivityDetailViewModel : ViewModelBase
{
    private readonly IActivityFacade _activityFacade;
    public Guid ActivityId { get; set; }
    public Guid UserId { get; set; }
    public ActivityDetailModel Activity { get; set; } = ActivityDetailModel.Empty;
    public ActivityDetailViewModel(
        IMessengerService messengerService,
        IActivityFacade activityFacade)
        : base(messengerService)
    {
        _activityFacade = activityFacade;
    }
}