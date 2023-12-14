using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using WpfApp1.APP.Services.Interfaces;
using WpfApp1.BL.Facades.Interfaces;
using WpfApp1.BL.Models;

namespace WpfApp1.APP.ViewModels;

public partial class CreateUserViewModel : ViewModelBase
{
	private readonly INavigationService _navigationService;
	private readonly IUserFacade _userFacade;

	public UserDetailModel User { get; set; } = UserDetailModel.Empty;

	public CreateUserViewModel(INavigationService navigationService, IUserFacade userFacade)
	{
		_navigationService = navigationService;
		_userFacade = userFacade;
	}

	[RelayCommand]
	private async Task CreateUser()
	{
		if (User.UserName.Length < 3 || User.UserName.Length > 15)
		{
			MessageBox.Show("Username musí být dlouhý 3 až 15 znaků", "Hupsík dupsík...", MessageBoxButton.OK, MessageBoxImage.Error);
		}
		else
		{
			await _userFacade.SaveAsync(User);
			User = UserDetailModel.Empty;
			_navigationService.NavigateTo<HomeViewModel>();
		}
	}
	
	[RelayCommand]
	private void GoToHomeView()
	{
		User = UserDetailModel.Empty;
		_navigationService.NavigateTo<HomeViewModel>();
	}
}