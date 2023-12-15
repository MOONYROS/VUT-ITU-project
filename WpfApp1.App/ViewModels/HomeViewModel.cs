using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using WpfApp1.App;
using WpfApp1.App.Messages;
using WpfApp1.APP.Services.Interfaces;
using WpfApp1.BL.Facades.Interfaces;
using WpfApp1.BL.Models;
using WpfApp1.DAL.Repositories;

namespace WpfApp1.APP.ViewModels;

public partial class HomeViewModel : ViewModelBase,
	IRecipient<BootMessage>,
	IRecipient<UserCreatedMessage>,
	IRecipient<UserDeletedMessage>
{
	private readonly INavigationService _navigationService;
	private readonly IUserFacade _userFacade;
	private readonly IMessengerService _messengerService;
	private ISharedUserIdService _idService;
	public ObservableCollection<UserListModel> Users { get; set; }

	public HomeViewModel(
		INavigationService navigationService,
		IMessengerService messengerService,
		IUserFacade userFacade,
		ISharedUserIdService idService) : base(messengerService)
	{
		_navigationService = navigationService;
		_messengerService = messengerService;
		_userFacade = userFacade;
		_idService = idService;
		messengerService.Messenger.Register<UserCreatedMessage>(this);
		messengerService.Messenger.Register<BootMessage>(this);
		messengerService.Messenger.Register<UserDeletedMessage>(this);
	}

	[RelayCommand]
	private void GoToCreateUser()
	{
		_navigationService.NavigateTo<CreateUserViewModel>();
	}

	[RelayCommand]
	private void GoToActivityListView(Guid userGuid)
	{
		_idService.UserId = userGuid;
		_navigationService.NavigateTo<ActivityListViewModel>();
		_messengerService.Send(new NavigationMessage());
	}

	protected override async Task LoadDataAsync()
	{
		var tmpUsers = await _userFacade.GetAsync();
		Users = tmpUsers.ToObservableCollection();
	}

	public async void Receive(BootMessage message)
	{
		await LoadDataAsync();
	}

	public async void Receive(UserCreatedMessage message)
	{
		await LoadDataAsync();
	}

	public async void Receive(UserDeletedMessage message)
	{
		await LoadDataAsync();
	}
}