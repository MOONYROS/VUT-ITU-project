using project.App.ViewModels;

namespace project.App.Views;

public partial class ActivitiesView
{
    public ActivitiesView(ActivitiesViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}