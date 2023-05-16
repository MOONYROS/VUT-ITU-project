using CommunityToolkit.Mvvm.Input;
using project.App.Messages;
using project.App.Services;
using project.App.Services.Interfaces;
using project.BL.Facades.Interfaces;
using project.BL.Models;



namespace project.App.ViewModels
{
    public partial class AddUserViewModel : ViewModelBase
    {
        private readonly IUserFacade _userFacade;
        private readonly INavigationService _navigationService;
        public UserDetailModel User { get; set; } = UserDetailModel.Empty;
        public AddUserViewModel(IMessengerService messengerService, 
            IUserFacade userFacade,
            INavigationService navigationService
            ) : base(messengerService)
        {
            _userFacade = userFacade;
            _navigationService = navigationService;
        }

        [RelayCommand]
        public async Task SaveUserAsync()
        {
            if (User.UserName.Length > 3 && User.UserName.Length < 16) 
            { 
                await _userFacade.SaveAsync(User);
                messengerService.Send(new UserAddMessage());
                _navigationService.SendBackButtonPressed();
            }
        }
    }
}
