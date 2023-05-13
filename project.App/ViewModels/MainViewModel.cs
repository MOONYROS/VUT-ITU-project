using CommunityToolkit.Mvvm.Input;

namespace project.App.ViewModels
{

    public partial class MainViewModel : ViewModelBase
    {
        [RelayCommand]
        private void GoToAddUser()
        {
            Shell.Current.GoToAsync("main/newUser");
        }
    }
}