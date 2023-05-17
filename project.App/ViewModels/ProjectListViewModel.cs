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
    IRecipient<ProjectAddMessage>,
    IRecipient<ProjectDeleteMessage>
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

    protected override async Task LoadDataAsync()
    {
        var todos = await _projectFacade.GetAsync();
        Projects = Projects.ToObservableCollection();
    }

    [RelayCommand]
    private async void GoToAddProject()
    {
        await _navigationService.GoToAsync<AddProjectViewModel>(
                new Dictionary<string, object?> { [nameof(AddProjectViewModel.UserId)] = UserId });
    }
    public async void Receive(ProjectAddMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(ProjectDeleteMessage message)
    {
        await LoadDataAsync();
    }
}