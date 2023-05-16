using CommunityToolkit.Mvvm.Input;
using project.App.Messages;
using project.App.Services.Interfaces;
using project.BL.Facades.Interfaces;
using project.BL.Models;
using System.Collections.ObjectModel;

namespace project.App.ViewModels
{
    public partial class ActivitiesListViewModel : ViewModelBase
    {
        private readonly IActivityFacade _activityFacade;
        private readonly INavigationService _navigationService;
        public ObservableCollection<ActivityListModel> Activities { get; set; } = null;

        public ActivitiesListViewModel(
            IMessengerService messengerService,
            IActivityFacade activityFacade,
            INavigationService navigationService
            ) : base(messengerService)
        {
            _activityFacade = activityFacade;
            _navigationService = navigationService;
        }

        protected async Task LoadDataAsync(Guid Id)
        {
            var act = await _activityFacade.GetAsyncUser(Id);
            Activities = act.ToObservableCollection();
        }
        public async void Receive(ActivityDeleteMessage message)
        {
            await LoadDataAsync();
        }

        [RelayCommand]
        private async void GoToAddActivity()
        {
            Console.WriteLine("Debug");
            await _navigationService.GoToAsync("main/activities/userActivities/addActivity");
        }
    }
}
