using CommunityToolkit.Mvvm.Messaging;
using project.App.Messages;
using project.App.Services.Interfaces;
using project.BL.Facades.Interfaces;
using project.BL.Models;
using System.Collections.ObjectModel;

namespace project.App.ViewModels;

[QueryProperty(nameof(UserId), nameof(UserId))]
public class TodoListViewModel : ViewModelBase,
    IRecipient<TodoAddMessage>,
    IRecipient<TodoDeleteMessage>
{
    private readonly ITodoFacade _todoFacade;
    public Guid UserId { get; set; }
    public ObservableCollection<TodoDetailModel> Todos { get; set; } = new();
    public TodoListViewModel(IMessengerService messengerService,
        ITodoFacade todoFacade) : base(messengerService)
    {
        _todoFacade = todoFacade;
    }

    protected override async Task LoadDataAsync()
    {
        var todos = await _todoFacade.GetAsyncUser(UserId);
        Todos = todos.ToObservableCollection();
    }

    public async void Receive(TodoAddMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(TodoDeleteMessage message)
    {
        await LoadDataAsync();
    }
}