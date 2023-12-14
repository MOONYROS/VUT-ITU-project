using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using WpfApp1.APP.Services.Interfaces;
using WpfApp1.BL.Facades.Interfaces;
using WpfApp1.BL.Models;

namespace WpfApp1.APP.ViewModels;

public partial class CreateUserViewModel : ViewModelBase
{
	private readonly INavigationService _navigationService;
	private readonly IUserFacade _userFacade;

	public UserDetailModel User { get; set; } = UserDetailModel.Empty;

	public CreateUserViewModel(INavigationService navigationService, IUserFacade userFacade)
	{
		_navigationService = navigationService;
		_userFacade = userFacade;
	}

	[RelayCommand]
	private async Task CreateUser()
	{
		await _userFacade.SaveAsync(User);
		User = UserDetailModel.Empty;
		_navigationService.NavigateTo<HomeViewModel>();
	}

	protected override async Task LoadDataAsync()
	{
		await _userFacade.GetAsync(User.Id);
	}
}