using project.App.ViewModels;

namespace project.App.Views;

public partial class ActivitiesListView
{
	public ActivitiesListView(ActivitiesViewModel viewModel) : base(viewModel)
	{
		InitializeComponent();
	}
}