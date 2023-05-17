using CommunityToolkit.Mvvm.Messaging;
using project.App.ViewModels;

namespace project.App.ViewModels
{
    public partial class ActivityDetailViewModel : ViewModelBase, IRecipient<ActivityDetailMesagge>
    {
    }
}
