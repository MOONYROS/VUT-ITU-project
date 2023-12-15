using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using WpfApp1.App.Messages;
using WpfApp1.APP.Services.Interfaces;
using WpfApp1.BL.Facades.Interfaces;
using WpfApp1.BL.Models;
using WpfApp1.App;

namespace WpfApp1.APP.ViewModels;

public partial class ActivityListViewModel : ViewModelBase,
	IRecipient<NavigationMessage>,
	IRecipient<ActivityAddedMessage>,
	IRecipient<LogOutMessage>
{
	private readonly IActivityFacade _ActivityFacadeFacade;
	private readonly ITagFacade _tagFacade;
	private readonly IMessengerService _messengerService;
	private readonly INavigationService _navigationService;
	private ISharedUserIdService _idService;
	private bool _firstLoad = true;

	public DateTime From { get; set; } = DateTime.Now;
	public DateTime To { get; set; } = DateTime.Now;

	public TagDetailModel SelectedTag { get; set; }

	public ObservableCollection<TagDetailModel> Tags { get; set; } = new();
	public ObservableCollection<ActivityListModel> Activities { get; set; } = new();
	
	public ActivityListViewModel(
		IMessengerService messengerService,
		IActivityFacade ActivityFacadeFacade,
		INavigationService navigationService,
		ISharedUserIdService idService, 
		ITagFacade tagFacade) : base(messengerService)
	{
		_messengerService = messengerService;
		_ActivityFacadeFacade = ActivityFacadeFacade;
		_navigationService = navigationService;
		_idService = idService;
		_tagFacade = tagFacade;
		_messengerService.Messenger.Register<NavigationMessage>(this);
		_messengerService.Messenger.Register<ActivityAddedMessage>(this);
		_messengerService.Messenger.Register<LogOutMessage>(this);
	}

	protected override async Task LoadDataAsync()
	{
		var tmpTags = await _tagFacade.GetAsyncUser(_idService.UserId);
		Tags = tmpTags.ToObservableCollection();
		Tags.Insert(0, TagDetailModel.Empty);
		SelectedTag = Tags.First();
	}
	
	[RelayCommand]
	private void ApplyFilter()
	{
		Guid? TagId;
		try
		{
			if (SelectedTag.Name == String.Empty)
			{
				TagId = null;
			}
			else
			{
				TagId = SelectedTag.Id;
			}
		}
		catch (Exception e)
		{
			TagId = null;
		}
		if (DateTime.Compare(From, To) < 0)
		{
			
		}
		else
		{
			MessageBox.Show("\"From\" je pozdeji nez \"To\"... Co to zas zkousis...",
				"Hupsík dupsík...", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
	
	[RelayCommand]
	private void GoToTagListView()
	{
		_navigationService.NavigateTo<TagListViewModel>();
		_messengerService.Send(new NavigationMessage());
	}
	
	[RelayCommand]
	private void GoToTodoListView()
	{
		_navigationService.NavigateTo<TodoListViewModel>();
		_messengerService.Send(new NavigationMessage());
	}
	
	[RelayCommand]
	private void GoToEditUserView()
	{
		_navigationService.NavigateTo<EditUserViewModel>();
		_messengerService.Send(new NavigationMessage());
	}

	
	[RelayCommand]
	private void LogOut()
	{
		_idService.UserId = Guid.Empty;
		_navigationService.NavigateTo<HomeViewModel>();
		_messengerService.Send(new LogOutMessage());
	}

	public async void Receive(NavigationMessage message)
	{
		if (_firstLoad)
		{
			_firstLoad = false;
			await LoadDataAsync();
		}
	}
	
	public void Receive(LogOutMessage message)
	{
		_firstLoad = true;
	}
	
	public void Receive(ActivityAddedMessage message) => throw new System.NotImplementedException();
}