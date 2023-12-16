using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using WpfApp1.App.Messages;
using WpfApp1.APP.Services.Interfaces;
using WpfApp1.BL.Facades.Interfaces;
using WpfApp1.BL.Models;

namespace WpfApp1.APP.ViewModels;

public partial class EditUserViewModel : ViewModelBase,
	IRecipient<NavigationMessage>,
	IRecipient<LogOutMessage>
{
	private readonly INavigationService _navigationService;
	private readonly IUserFacade _userFacade;
	private readonly IMessengerService _messengerService;
	private readonly ISharedUserIdService _idService;
	private bool _firstLoad = true;

	public UserDetailModel User { get; private set; } = UserDetailModel.Empty;

	public EditUserViewModel(
		IMessengerService messengerService, 
		INavigationService navigationService, 
		IUserFacade userFacade,
		ISharedUserIdService idService)
	{
		_messengerService = messengerService;
		_navigationService = navigationService;
		_userFacade = userFacade;
		_idService = idService;
		messengerService.Messenger.Register<NavigationMessage>(this);
		messengerService.Messenger.Register<LogOutMessage>(this);
	}
	
	protected override async Task LoadDataAsync()
	{
		User = await _userFacade.GetAsync(_idService.UserId);
	}
	
	public async void Receive(NavigationMessage message)
	{
		if (_firstLoad)
		{
			_firstLoad = false;
			await LoadDataAsync();
		}
	}
	
	[RelayCommand]
	private async Task GoToTodoListView()//dismiss
	{
		User = await _userFacade.GetAsync(_idService.UserId); 
		_navigationService.NavigateTo<ActivityListViewModel>();
	}
	
	[RelayCommand]
	private async Task DeleteUser()
	{
		if (MessageBox.Show("Opravdu chcete smazat uživatele?",
			    "Joooo fakt??!?",
			    MessageBoxButton.YesNo,
			    MessageBoxImage.Question) == MessageBoxResult.Yes)
		{
			await _userFacade.DeleteAsync(_idService.UserId);
			_messengerService.Send(new UserDeletedMessage());
			_messengerService.Send(new LogOutMessage());
			_navigationService.NavigateTo<HomeViewModel>();
		}
	}
	
	[RelayCommand]
	private async Task EditUser()
	{
		await _userFacade.SaveAsync(User);
		_messengerService.Send(new UserCreatedMessage());
		_navigationService.NavigateTo<ActivityListViewModel>();
	}

	public void Receive(LogOutMessage message)
	{
		_firstLoad = true;
	}
}