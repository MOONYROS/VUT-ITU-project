using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WpfApp1.App.Messages;
using WpfApp1.APP.Services.Interfaces;
using WpfApp1.BL.Facades.Interfaces;
using WpfApp1.BL.Models;

namespace WpfApp1.APP.ViewModels;

public partial class CreateTodoViewModel : ViewModelBase
{
	private readonly ISharedUserIdService _idService;
	private readonly IMessengerService _messengerService;
	private readonly ITodoFacade _todoFacade;
	private readonly INavigationService _navigationService;
	public TodoDetailModel Todo { get; private set; } = TodoDetailModel.Empty;

	public CreateTodoViewModel(
		IMessengerService messengerService,
		ITodoFacade todoFacade,
		ISharedUserIdService idService,
		INavigationService navigationService)
		: base(messengerService)
	{
		_messengerService = messengerService;
		_todoFacade = todoFacade;
		_idService = idService;
		_navigationService = navigationService;
	}

	[RelayCommand]
	private void GoToTodoListView()
	{
		_navigationService.NavigateTo<TodoListViewModel>();
	}

	[RelayCommand]
	private void CreateTodo()
	{
		_todoFacade.SaveAsync(Todo, _idService.UserId);
		Todo = TodoDetailModel.Empty;
		_navigationService.NavigateTo<TodoListViewModel>();
		_messengerService.Send(new TodoAddedMessage());
	}
}