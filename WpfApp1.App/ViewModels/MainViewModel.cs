using System;
using WpfApp1.APP.Services;
using WpfApp1.APP.Services.Interfaces;

namespace WpfApp1.APP.ViewModels;

public partial class MainViewModel : ViewModelBase
{
	public INavigationService NavigationService { get; set; }
	public MainViewModel(
		INavigationService navigationService,
		HomeViewModel starting) : base()
	{
		NavigationService = navigationService;
		NavigationService.CurrentViewModel = starting;
	}
}