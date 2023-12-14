using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using WpfApp1.App;
using WpfApp1.App.Messages;
using WpfApp1.APP.Services.Interfaces;
using WpfApp1.App.Views;
using WpfApp1.BL.Facades.Interfaces;
using WpfApp1.BL.Models;

namespace WpfApp1.APP.ViewModels;

public partial class TodoListViewModel : ViewModelBase,
	IRecipient<NavigationMessage>,
	IRecipient<TodoAddedMessage>,
	IRecipient<LogOutMessage>
{
	private bool _firstLoad = true;
	private readonly ITodoFacade _todoFacade;
	private readonly IMessengerService _messengerService;
	private readonly INavigationService _navigationService;
	private ISharedUserIdService _idService;

	public ObservableCollection<TodoDetailModel> Todos { get; set; } = new();
	public TodoListViewModel(
		IMessengerService messengerService,
		ITodoFacade todoFacade,
		INavigationService navigationService, 
		ISharedUserIdService idService)
		: base(messengerService)
	{
		_messengerService = messengerService;
		_todoFacade = todoFacade;
		_navigationService = navigationService;
		_idService = idService;
		_messengerService.Messenger.Register<NavigationMessage>(this);
		_messengerService.Messenger.Register<TodoAddedMessage>(this);
		_messengerService.Messenger.Register<LogOutMessage>(this);
	}

	protected override async Task LoadDataAsync()
	{
		var bruh = await _todoFacade.GetAsyncUser(_idService.UserId);
		Todos = bruh.ToObservableCollection();
	}
	
	[RelayCommand]
	private async void test()
	{
		var tmpTodo = new TodoDetailModel
		{
			Name = "Todo Brother",
			Date = default,
			Finished = false
		};
		await _todoFacade.SaveAsync(tmpTodo, _idService.UserId);
		_messengerService.Send(new TodoAddedMessage());
	}
	
		
	[RelayCommand]
	private void GoToTagListView()
	{
		_navigationService.NavigateTo<TagListViewModel>();
		_messengerService.Send(new NavigationMessage());
	}

	[RelayCommand]
	private void LogOut()
	{
		_idService.UserId = Guid.Empty;
		_messengerService.Send(new LogOutMessage());
		_navigationService.NavigateTo<HomeViewModel>();
	}

	public async void Receive(NavigationMessage message)
	{
		if (_firstLoad)
		{
			await LoadDataAsync();
			_firstLoad = false;
		}
	}

	public async void Receive(TodoAddedMessage message)
	{
		await LoadDataAsync();
	}

	public void Receive(LogOutMessage message)
	{
		_firstLoad = true;
	}
}