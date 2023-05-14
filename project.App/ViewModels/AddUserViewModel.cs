using project.App.Services.Interfaces;

namespace project.App.ViewModels
{
    public partial class AddUserViewModel : ViewModelBase
    {
        public AddUserViewModel(IMessengerService messengerService) : base(messengerService)
        {
        }
    }
}
