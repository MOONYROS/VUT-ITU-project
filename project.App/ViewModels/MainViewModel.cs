using CommunityToolkit.Mvvm.Input;
using project.BL.Models;
using project.App.Services.Interfaces;
using project.BL.Facades.Interfaces;
using CommunityToolkit.Mvvm.Messaging;
using project.App.Messages;
using System.Collections.ObjectModel;

namespace project.App.ViewModels
{
    public partial class MainViewModel : ViewModelBase, IRecipient<UserAddMessage>
    {
        private IUserFacade _userFacade { get; init; }
        public Guid Id { get; set; }
        public ObservableCollection<UserListModel> Users { get; set; } 
        
        public MainViewModel(
            IMessengerService messengerService,
            IUserFacade userFacade
            ) : base(messengerService)
        {
            _userFacade = userFacade;
        }

        [RelayCommand]
        private async void GoToAddUser()
        {
            await Shell.Current.GoToAsync("main/newUser");
        }
        [RelayCommand]
        private async void GoToActivities()
        {
            await Shell.Current.GoToAsync("main/activities");
        }

        protected override async Task LoadDataAsync()
        {
            IEnumerable<UserListModel> tmpUsers = await _userFacade.GetAsync();
            Users = tmpUsers.ToObservableCollection();
        }

        public async void Receive(UserAddMessage message)
        {
            await LoadDataAsync();
        }
    }
}