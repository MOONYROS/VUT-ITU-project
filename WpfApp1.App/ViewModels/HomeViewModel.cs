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
	IRecipient<UserCreatedMessage>
{
	private readonly INavigationService _navigationService;
	private readonly IUserFacade _userFacade;
	public ObservableCollection<UserListModel> Users { get; set; }

	public HomeViewModel(
		INavigationService navigationService,
		IMessengerService messengerService,
		IUserFacade userFacade) : base(messengerService)
	{
		_navigationService = navigationService;
		_userFacade = userFacade;
		messengerService.Messenger.Register<UserCreatedMessage>(this);
		messengerService.Messenger.Register<BootMessage>(this);
	}

	[RelayCommand]
	private void GoToCreateUser()
	{
		_navigationService.NavigateTo<CreateUserViewModel>();
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
}