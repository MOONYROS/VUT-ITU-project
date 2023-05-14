using CommunityToolkit.Mvvm.Input;
using project.BL.Models;
using project.App.Services.Interfaces;

namespace project.App.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        public Guid Id { get; set; }

        public MainViewModel(IMessengerService messengerService) : base(messengerService)
        {

        }
        public IEnumerable<UserListModel> Users { get; set; } = null!;

        public static UserDetailModel UserSeed() => new()
        {
            Id = Guid.NewGuid(),
            FullName = "Random name",
            UserName = "UserName"
        };
        
        [RelayCommand]
        private void GoToAddUser()
        {
            Shell.Current.GoToAsync("main/newUser");
        }
        [RelayCommand]
        private void GoToActivities()
        {
            Shell.Current.GoToAsync("main/activities");
        }
    }
}