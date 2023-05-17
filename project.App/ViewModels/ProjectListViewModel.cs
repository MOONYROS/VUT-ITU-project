using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using project.App.Messages;
using project.App.Services.Interfaces;
using project.App.ViewModels;
using project.BL.Facades;
using project.BL.Facades.Interfaces;
using project.BL.Models;
using System.Collections.ObjectModel;

namespace project.App.Views;

[QueryProperty(nameof(UserId), nameof(UserId))]
public partial class ProjectListViewModel : ViewModelBase,
    IRecipient<TodoAddMessage>,
    IRecipient<TodoDeleteMessage>
{
    private readonly IProjectFacade _projectFacade;
    private readonly INavigationService _navigationService;
    public Guid UserId { get; set; }

    public ObservableCollection<ProjectDetailModel> Projects { get; set; } = new();
    public ProjectListViewModel(IMessengerService messengerService,
        IProjectFacade projectFacade,
        INavigationService navigationService) : base(messengerService)
    {
        _navigationService = navigationService;
        _projectFacade = projectFacade;
    }

    [RelayCommand]
    private async void GoToAddProject()
    {
        await _navigationService.GoToAsync("main/activities/userProject/addProject");
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