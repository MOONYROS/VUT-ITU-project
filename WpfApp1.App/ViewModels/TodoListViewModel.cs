using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using WpfApp1.App;
using WpfApp1.App.Messages;
using WpfApp1.APP.Services.Interfaces;
using WpfApp1.BL.Facades.Interfaces;
using WpfApp1.BL.Models;

namespace WpfApp1.APP.ViewModels;

public partial class TodoListViewModel : ViewModelBase,
	IRecipient<TodoListNavigateMessage>
{
	private Guid _userGuid = Guid.Empty;
	private readonly ITodoFacade _todoFacade;
	public ObservableCollection<TodoDetailModel> Todos { get; set; } = new();
	public TodoListViewModel(IMessengerService messengerService,
		ITodoFacade todoFacade)
		: base(messengerService)
	{
		_todoFacade = todoFacade;
	}

	protected override async Task LoadDataAsync()
	{
		var bruh = await _todoFacade.GetAsyncUser(_userGuid);
		Todos = bruh.ToObservableCollection();
	}
	public async void Receive(TodoListNavigateMessage message)
	{
		_userGuid = message.UserGuid;
		await LoadDataAsync();
	}
}