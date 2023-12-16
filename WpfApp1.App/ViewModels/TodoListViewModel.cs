using System;
using System.Collections.ObjectModel;
using System.Linq;
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
	private readonly ISharedUserIdService _idService;

	public ObservableCollection<TodoDetailModel> UnfinishedTodos { get; private set; } = new();
	public ObservableCollection<TodoDetailModel> FinishedTodos { get; private set; } = new();

	public TodoListViewModel(
		IMessengerService messengerService,
		ITodoFacade todoFacade,
		INavigationService navigationService, 
		ISharedUserIdService idService)
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
		var bruh = await _todoFacade.GetAsyncUser(_idService.UserId, false);
		UnfinishedTodos = bruh.ToObservableCollection();
		bruh = await _todoFacade.GetAsyncUser(_idService.UserId, true);
		FinishedTodos = bruh.ToObservableCollection();
	}
	
	[RelayCommand]
	private void GoToEditUserView()
	{
		_navigationService.NavigateTo<EditUserViewModel>();
		_messengerService.Send(new NavigationMessage());
	}

	[RelayCommand]
	private void GoToTagListView()
	{
		_navigationService.NavigateTo<TagListViewModel>();
		_messengerService.Send(new NavigationMessage());
	}

	[RelayCommand]
	private void GoToCreateTodoView()
	{
		_navigationService.NavigateTo<CreateTodoViewModel>();
	}

	[RelayCommand]
	private void GoToActivityListView()
	{
		_navigationService.NavigateTo<ActivityListViewModel>();
	}

	[RelayCommand]
	private void LogOut()
	{
		_idService.UserId = Guid.Empty;
		_messengerService.Send(new LogOutMessage());
		_navigationService.NavigateTo<HomeViewModel>();
	}

	[RelayCommand]
	private async Task FinishToDo(Guid todoId)
	{
		var tmpTodo = UnfinishedTodos.FirstOrDefault(element => element.Id == todoId);
		tmpTodo.Finished = true;
		await _todoFacade.SaveAsync(tmpTodo, _idService.UserId);
		await LoadDataAsync();
	}

	[RelayCommand]
	private async Task UnfinishToDo(Guid todoId)
	{
		var tmpTodo = FinishedTodos.FirstOrDefault(element => element.Id == todoId);
		tmpTodo.Finished = false;
		await _todoFacade.SaveAsync(tmpTodo, _idService.UserId);
		await LoadDataAsync();
	}

	[RelayCommand]
	private async Task DeleteTodo(Guid todoId)
	{
		await _todoFacade.DeleteAsync(todoId);
		await LoadDataAsync();
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