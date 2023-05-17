using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using project.App.Messages;
using project.App.Services.Interfaces;
using project.BL.Facades;
using project.BL.Facades.Interfaces;
using project.BL.Models;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.JavaScript;

namespace project.App.ViewModels;

[QueryProperty(nameof(UserId), nameof(UserId))]
public partial class AddTodoViewModel : ViewModelBase
{
    private readonly ITodoFacade _todoFacade;
    private readonly INavigationService _navigationService;
    private readonly IAlertService _alertService;
    public Guid UserId { get; set; }

    public DateTime Time { get; set; }
    public TodoDetailModel Todo { get; set; } = TodoDetailModel.Empty;

    public AddTodoViewModel(
        IMessengerService messengerService,
        ITodoFacade todoFacade, 
        INavigationService navigationService,
        IAlertService alertService) 
        : base(messengerService)
    {
        _todoFacade = todoFacade;
        _navigationService = navigationService;
        _alertService = alertService;
    }

    [RelayCommand]
    public async Task SaveTodoAsync()
    {
        if (Todo.Date < DateOnly.FromDateTime(DateTime.Now))
        {
            await _alertService.DisplayAsync("Hupsik Dupsik", "too late m8th");
        }
        else
        {
            Todo.Finished = false;
            Todo.Date = DateOnly.FromDateTime(Time);
            await _todoFacade.SaveAsync(Todo, UserId);
            messengerService.Send(new TodoAddMessage());
            await _navigationService.GoToAsync<TodoListViewModel>(
                new Dictionary<string, object?> { [nameof(AddTodoViewModel.UserId)] = UserId });
        }
        
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