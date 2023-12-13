using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using WpfApp1.APP.Models;
using WpfApp1.APP.Services.Interfaces;
using WpfApp1.APP.ViewModels.Interfaces;

namespace WpfApp1.APP.Services;

public class NavigationService : INavigationService
{
	private readonly Frame _mainFrame;

	public NavigationService(Frame mainFrame)
	{
		_mainFrame = mainFrame ?? throw new ArgumentNullException(nameof(mainFrame));
	}

	public IEnumerable<RouteModel> Routes { get; } = new List<RouteModel>
	{

	};

	public void GoTo<TViewModel>() where TViewModel : IViewModel
	{
		var route = GetRouteByViewModel<TViewModel>();
		NavigateTo(route);
	}

	public void GoTo<TViewModel>(IDictionary<string, object?>? parameters) where TViewModel : IViewModel
	{
		var route = GetRouteByViewModel<TViewModel>();
		NavigateTo(route, parameters);
	}

	public void GoTo(string route, IDictionary<string, object?>? parameters)
	{
		NavigateTo(route, parameters);
	}

	private string GetRouteByViewModel<TViewModel>() where TViewModel : IViewModel
	{
		return Routes.First(route => route.ViewModelType == typeof(TViewModel)).Route;
	}

	private void NavigateTo(string route, IDictionary<string, object?>? parameters = null)
	{
		var uri = new Uri(route, UriKind.Absolute);
		_mainFrame.Navigate(uri, parameters);
	}
}