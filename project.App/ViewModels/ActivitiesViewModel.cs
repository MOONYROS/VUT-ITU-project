using project.App.Services.Interfaces;

namespace project.App.ViewModels
{
    public partial class ActivitiesViewModel : ViewModelBase
    {
        public IEnumerable<DateTime> Week { get; set; }
        public DateTime Today { get; set; }

        public ActivitiesViewModel(IMessengerService messengerService)
            :base(messengerService)
        {
            Week = new List<DateTime>();
            Today = DateTime.Today;
            while (Today.DayOfWeek != DayOfWeek.Monday)
            {
                Today = Today.AddDays(-1);
            }

            for (int i = 0; i < 7; i++)
            {
                Week = Week.Append(Today);
                Today = Today.AddDays(1);
            }
        }

    }
}
