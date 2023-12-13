using System.Collections.Generic;
using System.Threading.Tasks;
using WpfApp1.APP.Models;
using WpfApp1.APP.ViewModels.Interfaces;

namespace WpfApp1.APP.Services.Interfaces;

public interface INavigationService
{
	IEnumerable<RouteModel> Routes { get; }
	public void GoTo<TViewModel>() where TViewModel : IViewModel;
	public void GoTo<TViewModel>(IDictionary<string, object?>? parameters) where TViewModel : IViewModel;

}