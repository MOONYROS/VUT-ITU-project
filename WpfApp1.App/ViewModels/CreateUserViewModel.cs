using WpfApp1.APP.Services.Interfaces;

namespace WpfApp1.APP.ViewModels;

public partial class CreateUserViewModel : ViewModelBase
{
	private readonly INavigationService _navigationService;

	public CreateUserViewModel(INavigationService navigationService)
	{
		_navigationService = navigationService;
	}
}