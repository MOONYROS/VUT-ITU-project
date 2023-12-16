using System;
using System.IO;
using System.Windows;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
using WpfApp1.BL.Models;
using WpfApp1.DAL;
using WpfApp1.DAL.Factories;
using WpfApp1.DAL.Mappers;
using WpfApp1.DAL.UnitOfWork;

namespace WpfApp1.App;

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
			.FromAssemblyOf<BusinessLogic>()
			.AddClasses(filter => filter.AssignableTo<IModel>())
			.AsSelf()
			.WithTransientLifetime());

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
		serviceCollection.AddTransient<ActivityEditView>(provider => new ActivityEditView
		{
			DataContext = provider.GetRequiredService<ActivityEditViewModel>()
		});
		serviceCollection.AddTransient<ActivityListView>(provider => new ActivityListView
		{
			DataContext = provider.GetRequiredService<ActivityEditViewModel>()
		});
		serviceCollection.AddTransient<CreateActivityView>(provider => new CreateActivityView
		{
			DataContext = provider.GetRequiredService<CreateActivityViewModel>()
		});
		serviceCollection.AddTransient<CreateTagView>(provider => new CreateTagView
		{
			DataContext = provider.GetRequiredService<CreateTagViewModel>()
		});
		serviceCollection.AddTransient<CreateTodoView>(provider => new CreateTodoView
		{
			DataContext = provider.GetRequiredService<CreateTodoViewModel>()
		});
		serviceCollection.AddTransient<EditUserView>(provider => new EditUserView
		{
			DataContext = provider.GetRequiredService<EditUserViewModel>()
		});
		serviceCollection.AddTransient<TagListView>(provider => new TagListView
		{
			DataContext = provider.GetRequiredService<TagListViewModel>()
		});
		serviceCollection.AddTransient<TodoListView>(provider => new TodoListView
		{
			DataContext = provider.GetRequiredService<TodoListViewModel>()
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
