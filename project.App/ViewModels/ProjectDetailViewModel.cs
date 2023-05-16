using CommunityToolkit.Mvvm.Input;
using project.App.Services.Interfaces;
using project.BL.Facades.Interfaces;
using project.BL.Models;

namespace project.App.ViewModels;

public class ProjectDetailViewModel : ViewModelBase
{
    private readonly IUserFacade _userFacade;
    private readonly IProjectFacade _projectFacade;

    public ProjectDetailViewModel(IMessengerService messengerService,
       IUserFacade userFacade,
       IProjectFacade projectFacade) : base(messengerService)
    {
        _userFacade = userFacade;
        _projectFacade = projectFacade;
    }

    
}
