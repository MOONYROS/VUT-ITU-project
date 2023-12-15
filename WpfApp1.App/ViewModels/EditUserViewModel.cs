using System.Threading.Tasks;
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
	private ISharedUserIdService _idService;
	private bool _firstLoad = true;

	public UserDetailModel User { get; set; } = UserDetailModel.Empty;

	public EditUserViewModel(
		IMessengerService messengerService, 
		INavigationService navigationService, 
		IUserFacade userFacade,
		ISharedUserIdService idService) : base(messengerService)
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
	private async void GoToTodoListView()//dismiss
	{
		User = await _userFacade.GetAsync(_idService.UserId); 
		_navigationService.NavigateTo<TodoListViewModel>();
	}
	
	[RelayCommand]
	private async Task DeleteUser()
	{
		await _userFacade.DeleteAsync(_idService.UserId);
		_messengerService.Send(new UserDeletedMessage());
		_messengerService.Send(new LogOutMessage());
		_navigationService.NavigateTo<HomeViewModel>();
	}
	
	[RelayCommand]
	private async Task EditUser()
	{
		await _userFacade.SaveAsync(User);
		_messengerService.Send(new UserCreatedMessage());
		_navigationService.NavigateTo<TodoListViewModel>();
	}


	public void Receive(LogOutMessage message)
	{
		_firstLoad = true;
	}
}