using CommunityToolkit.Mvvm.Input;
using project.App.Services.Interfaces;
using project.BL.Facades.Interfaces;
using project.BL.Models;
using project.App.Messages;

namespace project.App.ViewModels;
    public partial class ActivityListViewModel : ViewModelBase
    {
        private readonly IActivityFacade _activityFacade;
        private readonly INavigationService _navigationService;
        public IEnumerable<ActivityListModel> Activities { get; set; } = null;

        public ActivityListViewModel(IMessengerService messengerService,
            IActivityFacade activityFacade,
            INavigationService navigationService) : base(messengerService)
        {
            _activityFacade = activityFacade;
            _navigationService = navigationService;
        }

        protected override async Task LoadDataAsync()
        {
            await base.LoadDataAsync();

            Activities = await _activityFacade.GetAsync();
        }
        public async void Receive(ActivityDeleteMessage message)
        {
            await LoadDataAsync();
        }

}
