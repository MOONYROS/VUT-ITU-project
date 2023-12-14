using System;
using CommunityToolkit.Mvvm.ComponentModel;
using WpfApp1.APP.Services.Interfaces;
using WpfApp1.APP.ViewModels;

namespace WpfApp1.APP.Services;

public class NavigationService : ObservableObject, INavigationService
{
	private readonly Func<Type, ViewModelBase> _viewModelFactory;
	private readonly IMessengerService _messengerService;

	public event Action CurrentViewModelChanged;
	private ViewModelBase _currentViewModel;

	public ViewModelBase CurrentViewModel
	{
		get => _currentViewModel;
		set 
		{
			_currentViewModel = value;
			OnPropertyChanged();
		}
	}

	private void OnCurrentViewModelChanged()
	{
		CurrentViewModelChanged?.Invoke();
	}

	public NavigationService(
		IMessengerService messengerService,
		Func<Type, ViewModelBase> viewModelFactory)
	{
		_messengerService = messengerService;
		_viewModelFactory = viewModelFactory;
	}

	public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
	{
		CurrentViewModel = _viewModelFactory.Invoke(typeof(TViewModel));
	}
}