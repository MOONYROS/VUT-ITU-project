using project.App.Services.Interfaces;

namespace project.App.ViewModels;

public class TodoListViewModel:ViewModelBase
{
    public TodoListViewModel(IMessengerService messengerService) : base(messengerService)
    {
    }
}