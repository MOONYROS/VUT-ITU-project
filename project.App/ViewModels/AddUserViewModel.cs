using CommunityToolkit.Mvvm.Input;
using project.App.Services.Interfaces;
using project.BL.Facades.Interfaces;
using project.BL.Models;

namespace project.App.ViewModels
{
    public partial class AddUserViewModel : ViewModelBase
    {
        private readonly IUserFacade _userFacade;
        private readonly UserDetailModel _user = UserDetailModel.Empty;
        public AddUserViewModel(IMessengerService messengerService, 
            IUserFacade userFacade) : base(messengerService)
        {
            _userFacade = userFacade;
        }
        
        [RelayCommand]
        public async Task SaveUserAsync()
        {
            await _userFacade.SaveAsync(_user);
        }
    }
}
