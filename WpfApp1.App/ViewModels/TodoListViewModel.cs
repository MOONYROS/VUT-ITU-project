using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using WpfApp1.App;
using WpfApp1.App.Messages;
using WpfApp1.APP.Services.Interfaces;
using WpfApp1.BL.Facades.Interfaces;
using WpfApp1.BL.Models;

namespace WpfApp1.APP.ViewModels;

public partial class TodoListViewModel : ViewModelBase,
	IRecipient<NavigationMessage>
{
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
		messengerService.Messenger.Register<NavigationMessage>(this);
	}

	protected override async Task LoadDataAsync()
	{
		var bruh = await _todoFacade.GetAsyncUser(_idService.UserId);
		Todos = bruh.ToObservableCollection();
	}
	
	[RelayCommand]
	private void test()
	{
		MessageBox.Show($"{_idService.UserId}", "jolol", MessageBoxButton.OK, MessageBoxImage.Error);
	}
	
		
	[RelayCommand]
	private void GoToTagListView()
	{
		_messengerService.Send(new NavigationMessage());
		_navigationService.NavigateTo<TagListViewModel>();
	}

	public async void Receive(NavigationMessage message)
	{
		await LoadDataAsync();
	}
}