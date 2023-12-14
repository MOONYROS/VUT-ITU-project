using System;
using WpfApp1.APP.Services;
using WpfApp1.APP.Services.Interfaces;

namespace WpfApp1.APP.ViewModels;

public partial class MainViewModel : ViewModelBase
{
	private readonly INavigationService _navigationService;

	public event Action CurrentViewModelChanged;
	public ViewModelBase CurrentViewModel => _navigationService.CurrentViewModel;

	public MainViewModel(
		INavigationService navigationService,
		HomeViewModel starting)
	{
		_navigationService = navigationService;
		_navigationService.CurrentViewModel = starting;
	}
}