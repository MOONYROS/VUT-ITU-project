using project.App.Services.Interfaces;

namespace project.App.ViewModels
{
    public partial class ActivitiesListViewModel : ViewModelBase
    {
        public ActivitiesListViewModel(IMessengerService messengerService) : base(messengerService)
        {
        }
    }
}
