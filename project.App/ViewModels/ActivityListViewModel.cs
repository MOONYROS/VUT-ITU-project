using CommunityToolkit.Mvvm.Input;
using project.App.Services.Interfaces;
using project.BL.Facades.Interfaces;
using project.BL.Models;
using project.App.Messages;
using System.Collections.ObjectModel;

namespace project.App.ViewModels;
    public partial class ActivityListViewModel : ViewModelBase
    {
        private readonly IActivityFacade _activityFacade;
        private readonly INavigationService _navigationService;
        public ObservableCollection<ActivityListModel> Activities { get; set; } = null;

        public ActivityListViewModel(IMessengerService messengerService,
            IActivityFacade activityFacade,
            INavigationService navigationService) : base(messengerService)
        {
            _activityFacade = activityFacade;
            _navigationService = navigationService;
        }

        protected override async Task LoadDataAsync()
        {
            var act = await _activityFacade.GetAsync();
            Activities = act.ToObservableCollection();
        }
        public async void Receive(ActivityDeleteMessage message)
        {
            await LoadDataAsync();
        }

}
