using WpfApp1.APP.ViewModels;

namespace WpfApp1.APP.Services.Interfaces;

public interface INavigationService
{
	public ViewModelBase CurrentViewModel { get; set; }
	public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase;
}