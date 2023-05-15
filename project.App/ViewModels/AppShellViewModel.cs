using CommunityToolkit.Mvvm.Input;
using project.App.Services.Interfaces;
using project.App.Services;

namespace project.App.ViewModels
{
    public partial class AppShellViewModel : ViewModelBase
    {
        public AppShellViewModel(IMessengerService messengerService) : base(messengerService)
        {

        }

        [RelayCommand]
        private async void GoToActivitiesList()
        {
            await Shell.Current.GoToAsync("main/activities/userActivities");
        }
    }
}