using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using WpfApp1.App.Messages;
using WpfApp1.APP.Services.Interfaces;
using WpfApp1.BL.Facades.Interfaces;
using WpfApp1.BL.Models;

namespace WpfApp1.APP.ViewModels;

public partial class ActivityListViewModel : ViewModelBase,
	IRecipient<NavigationMessage>,
	IRecipient<ActivityAddedMessage>,
	IRecipient<LogOutMessage>
{
	private readonly IActivityFacade _tagActivityFacadeFacade;
	private readonly IMessengerService _messengerService;
	private readonly INavigationService _navigationService;
	private ISharedUserIdService _idService;
	private bool _firstLoad = true;

	public ObservableCollection<ActivityListModel> Tags { get; set; } = new();
	
	public ActivityListViewModel(
		IMessengerService messengerService,
		IActivityFacade tagActivityFacadeFacade,
		INavigationService navigationService,
		ISharedUserIdService idService) : base(messengerService)
	{
		_messengerService = messengerService;
		_tagActivityFacadeFacade = tagActivityFacadeFacade;
		_navigationService = navigationService;
		_idService = idService;
		_messengerService.Messenger.Register<NavigationMessage>(this);
		_messengerService.Messenger.Register<ActivityAddedMessage>(this);
		_messengerService.Messenger.Register<LogOutMessage>(this);
	}
	
	[RelayCommand]
	private void GoToTagListView()
	{
		_navigationService.NavigateTo<TagListViewModel>();
		_messengerService.Send(new NavigationMessage());
	}
	
	[RelayCommand]
	private void GoToTodoListView()
	{
		_navigationService.NavigateTo<TodoListViewModel>();
		_messengerService.Send(new NavigationMessage());
	}
	
	[RelayCommand]
	private void GoToEditUserView()
	{
		_navigationService.NavigateTo<EditUserViewModel>();
		_messengerService.Send(new NavigationMessage());
	}

	
	[RelayCommand]
	private void LogOut()
	{
		_idService.UserId = Guid.Empty;
		_navigationService.NavigateTo<HomeViewModel>();
		_messengerService.Send(new LogOutMessage());
	}

	public async void Receive(NavigationMessage message)
	{
		if (_firstLoad)
		{
			_firstLoad = false;
			await LoadDataAsync();
		}
	}
	
	public void Receive(LogOutMessage message)
	{
		_firstLoad = true;
	}
	
	public void Receive(ActivityAddedMessage message) => throw new System.NotImplementedException();
}