using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using WpfApp1.App.Messages;
using WpfApp1.APP.Services.Interfaces;
using WpfApp1.BL.Facades.Interfaces;
using WpfApp1.BL.Models;

namespace WpfApp1.APP.ViewModels;

public partial class CreateUserViewModel : ViewModelBase
{
	private readonly INavigationService _navigationService;
	private readonly IUserFacade _userFacade;
	private readonly IMessengerService _messengerService;

	public UserDetailModel User { get; private set; } = UserDetailModel.Empty;

	public CreateUserViewModel(
		INavigationService navigationService,
		IUserFacade userFacade,
		IMessengerService messengerService)
	{
		_navigationService = navigationService;
		_userFacade = userFacade;
		_messengerService = messengerService;
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
			_messengerService.Send(new UserCreatedMessage());
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