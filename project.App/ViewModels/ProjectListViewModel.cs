using project.App.Services.Interfaces;

namespace project.App.ViewModels;

public class ProjectListViewModel:ViewModelBase
{
    public ProjectListViewModel(IMessengerService messengerService) : base(messengerService)
    {
    }
}