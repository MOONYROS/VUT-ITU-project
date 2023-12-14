using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using WpfApp1.APP.ViewModels.Interfaces;

namespace WpfApp1.APP.ViewModels;

public abstract class ViewModelBase : ObservableObject, IViewModel
{
	protected ViewModelBase()
	{

	}

	public Task OnAppearingAsync() => throw new System.NotImplementedException();
}