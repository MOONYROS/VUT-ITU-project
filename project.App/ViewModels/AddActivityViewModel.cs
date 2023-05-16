using CommunityToolkit.Mvvm.Input;
using project.App.Messages;
using project.App.Services.Interfaces;
using project.BL.Facades;
using project.BL.Facades.Interfaces;
using project.BL.Models;

namespace project.App.ViewModels
{
    public partial class AddActivityViewModel : ViewModelBase
    {
        private readonly IActivityFacade _activityFacade;
        private readonly INavigationService _navigationService;

        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }
        public ActivityDetailModel activityDetailModel { get; set; } = ActivityDetailModel.Empty;
        public AddActivityViewModel(IMessengerService messengerService,
            IActivityFacade activityFacade,
            INavigationService navigationService
            ) : base(messengerService)
        {
            _activityFacade = activityFacade;
            _navigationService = navigationService;
        }

        [RelayCommand]
        public async Task SaveActivityAsync(Guid user)
        {
            activityDetailModel.DateTimeFrom = activityDetailModel.DateTimeFrom + TimeFrom;
            activityDetailModel.DateTimeTo = activityDetailModel.DateTimeTo + TimeTo;
            await _activityFacade.SaveAsync(activityDetailModel, user, null);
            messengerService.Send(new ActivityAddMessage());
            activityDetailModel = ActivityDetailModel.Empty;
        }
    }
}
