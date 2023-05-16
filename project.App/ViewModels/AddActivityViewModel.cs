using project.App.Services.Interfaces;

namespace project.App.ViewModels
{
    public partial class AddActivityViewModel : ViewModelBase
    {
        public AddActivityViewModel(IMessengerService messengerService) : base(messengerService)
        {

        }
    }
}
