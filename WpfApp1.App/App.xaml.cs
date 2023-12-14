using System;
using System.Windows;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using WpfApp1.APP.Services;
using WpfApp1.APP.Services.Interfaces;
using WpfApp1.APP.ViewModels;
using WpfApp1.App.Views;

namespace WpfApp1.App;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	private readonly ServiceProvider _serviceProvider;
	public App()
	{
		IServiceCollection serviceCollection = new ServiceCollection();
		serviceCollection.AddSingleton<IMessenger, StrongReferenceMessenger>();

		serviceCollection.AddSingleton<IMessengerService, MessengerService>();

		serviceCollection.AddSingleton<Func<Type, ViewModelBase>>(provider =>
			viewModelType => (ViewModelBase)provider.GetRequiredService(viewModelType));

		serviceCollection.AddSingleton<INavigationService, NavigationService>();

		serviceCollection.AddSingleton<CreateUserViewModel>();
		serviceCollection.AddSingleton<HomeViewModel>();
		serviceCollection.AddSingleton<MainViewModel>();

		serviceCollection.AddSingleton<MainWindow>(provider => new MainWindow
		{
			DataContext = provider.GetRequiredService<MainViewModel>()
		});
		serviceCollection.AddSingleton<HomeView>(provider => new HomeView
		{
			DataContext = provider.GetRequiredService<HomeViewModel>()
		});
		serviceCollection.AddSingleton<CreateUserView>(provider => new CreateUserView
		{
			DataContext = provider.GetRequiredService<CreateUserViewModel>()
		});

		_serviceProvider = serviceCollection.BuildServiceProvider();
	}
	protected override void OnStartup(StartupEventArgs e)
	{
		var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
		mainWindow.Show();
		base.OnStartup(e);
	}

}
