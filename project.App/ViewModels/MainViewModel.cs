using CommunityToolkit.Mvvm.Input;
using project.BL.Facades.Interfaces;
using project.BL.Models;

namespace project.App.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly IUserFacade _userFacade;

        public MainViewModel(IUserFacade userFacade)
        {
            _userFacade = userFacade;
        }

        public IEnumerable<UserListModel> Users { get; set; } = null!;

        protected override async Task LoadDataAsync()
        {
            await base.LoadDataAsync();

        }

        [RelayCommand]
        private void GoToAddUser()
        {
            Shell.Current.GoToAsync("main/newUser");
        }
    }
}