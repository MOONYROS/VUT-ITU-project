using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using project.App.Messages;
using project.App.Services.Interfaces;
using project.BL.Facades;
using project.BL.Facades.Interfaces;
using project.BL.Models;
using System.Collections.ObjectModel;

namespace project.App.ViewModels;

[QueryProperty(nameof(UserId), nameof(UserId))]
public partial class AddTodoViewModel : ViewModelBase
{
    private readonly ITodoFacade _todoFacade;
    public Guid UserId { get; set; }

    public DateTime Time { get; set; }
    public TodoDetailModel Todo { get; set; } = TodoDetailModel.Empty;

    public AddTodoViewModel(IMessengerService messengerService,
        ITodoFacade todoFacade) : base(messengerService)
    {
        _todoFacade = todoFacade;
    }

    [RelayCommand]
    public async Task SaveTodoAsync()
    {
        Todo.Finished = false;
        Todo.Date = DateOnly.FromDateTime(Time);
        await _todoFacade.SaveAsync(Todo, UserId);
        messengerService.Send(new TodoAddMessage());
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