using System;
using System.IO;
using System.Reflection;
using System.Windows;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic.FileIO;
using WpfApp1.App.Messages;
using WpfApp1.APP.Services;
using WpfApp1.APP.Services.Interfaces;
using WpfApp1.APP.ViewModels;
using WpfApp1.APP.ViewModels.Interfaces;
using WpfApp1.App.Views;
using WpfApp1.BL.Facades.Interfaces;
using WpfApp1.BL;
using WpfApp1.BL.Facades;
using WpfApp1.BL.Mappers.Interfaces;
using WpfApp1.DAL;
using WpfApp1.DAL.Factories;
using WpfApp1.DAL.Mappers;
using WpfApp1.DAL.UnitOfWork;

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
		serviceCollection.AddSingleton<IMessenger>(idk => StrongReferenceMessenger.Default);

		serviceCollection.AddSingleton<IMessengerService, MessengerService>();

		serviceCollection.AddSingleton<Func<Type, ViewModelBase>>(provider =>
			viewModelType => (ViewModelBase)provider.GetRequiredService(viewModelType));

		serviceCollection.AddSingleton<INavigationService, NavigationService>();

		serviceCollection.AddSingleton<ISharedUserIdService, SharedUserIdService>();
		serviceCollection.AddSingleton<ISharedActivityIdService, SharedActivityIdService>();

		serviceCollection.AddSingleton<IActivityTagFacade, ActivityTagFacade>();

		serviceCollection.Scan(selector => selector
			.FromAssemblyOf<App>()
			.AddClasses(filter => filter.AssignableTo<IViewModel>())
			.AsSelf()
			.WithSingletonLifetime());

		serviceCollection.Scan(selector => selector
			.FromAssemblyOf<BusinessLogic>()
			.AddClasses(filter => filter.AssignableTo(typeof(IFacade<,,>)))
			.AsMatchingInterface()
			.WithSingletonLifetime());

		serviceCollection.Scan(selector => selector
			.FromAssemblyOf<BusinessLogic>()
			.AddClasses(filter => filter.AssignableTo(typeof(IFacadeDetailOnly<,>)))
			.AsMatchingInterface()
			.WithSingletonLifetime()
		);

		serviceCollection.Scan(selector => selector
			.FromAssemblyOf<BusinessLogic>()
			.AddClasses(filter => filter.AssignableTo(typeof(IModelMapper<,,>)))
			.AsMatchingInterface()
			.WithSingletonLifetime()
		);

		serviceCollection.Scan(selector => selector
			.FromAssemblyOf<BusinessLogic>()
			.AddClasses(filter => filter.AssignableTo(typeof(IModelMapperDetailOnly<,>)))
			.AsMatchingInterface()
			.WithSingletonLifetime()
		);

		string databaseFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "database.db");
		serviceCollection.AddSingleton<IDbContextFactory<ProjectDbContext>>(provider => new DbContextSqLiteFactory(databaseFilePath));
		serviceCollection.AddSingleton<IDbMigrator, SqliteDbMigrator>();

		serviceCollection.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();

		serviceCollection.Scan(selector => selector
			.FromAssemblyOf<DataAccessLayer>()
			.AddClasses(filter => filter.AssignableTo(typeof(IEntityIDMapper<>)))
			.AsSelf()
			.WithSingletonLifetime());

		serviceCollection.AddTransient<MainWindow>(provider => new MainWindow
		{
			DataContext = provider.GetRequiredService<MainViewModel>()
		});
		serviceCollection.AddTransient<HomeView>(provider => new HomeView
		{
			DataContext = provider.GetRequiredService<HomeViewModel>()
		});
		serviceCollection.AddTransient<CreateUserView>(provider => new CreateUserView
		{
			DataContext = provider.GetRequiredService<CreateUserViewModel>()
		});

		_serviceProvider = serviceCollection.BuildServiceProvider();
	}
	protected override void OnStartup(StartupEventArgs e)
	{
		_serviceProvider.GetRequiredService<IDbMigrator>().Migrate();
		var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
		_serviceProvider.GetRequiredService<IMessengerService>().Send(new BootMessage());
		mainWindow.Show();
		base.OnStartup(e);
	}

}
