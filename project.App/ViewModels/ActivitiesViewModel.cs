using project.App.Services.Interfaces;

namespace project.App.ViewModels
{
    public class ActivitiesViewModel : ViewModelBase
    {
        public ActivitiesViewModel(IMessengerService messengerService) : base(messengerService)
        {
        }
    }
}
