using CommunityToolkit.Mvvm.Messaging;
using project.App.Services.Interfaces;
using project.App.ViewModels;

namespace project.App.ViewModels
{
    public partial class ActivityDetailViewModel : ViewModelBase
    {
        public ActivityDetailViewModel(IMessengerService messengerService) : base(messengerService)
        {
        }
    }
}
