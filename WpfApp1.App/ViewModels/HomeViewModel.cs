using CommunityToolkit.Mvvm.Input;
using WpfApp1.APP.Services.Interfaces;

namespace WpfApp1.APP.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
	private readonly INavigationService _navigationService;

	public HomeViewModel(INavigationService navigationService)
	{
		_navigationService = navigationService;
	}

	[RelayCommand]
	private void GoToCreateUser()
	{
		_navigationService.NavigateTo<CreateUserViewModel>();
	}
}