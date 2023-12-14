using WpfApp1.APP.Services.Interfaces;

namespace WpfApp1.APP.ViewModels;

public class TodoListViewModel : ViewModelBase
{
	public TodoListViewModel(IMessengerService messengerService)
		: base(messengerService)
	{
	}
}