using System;
using System.Collections.Generic;
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
using WpfApp1.DAL.Entities;

namespace WpfApp1.APP.ViewModels;

public partial class ActivityListViewModel : ViewModelBase,
	IRecipient<NavigationMessage>,
	IRecipient<ActivityAddedMessage>,
	IRecipient<ActivityDeletedMessage>,
	IRecipient<TagAddedMessage>,
	IRecipient<LogOutMessage>
{
	private readonly IActivityFacade _activityFacade;
	private readonly ITagFacade _tagFacade;
	private readonly IActivityTagFacade _activityTagFacade;
	private readonly IMessengerService _messengerService;
	private readonly INavigationService _navigationService;
	private ISharedUserIdService _userIdService;
	private ISharedActivityIdService _activityIdService;
	private bool _firstLoad = true;

	public DateTime From { get; set; } = DateTime.Now;
	public DateTime To { get; set; } = DateTime.Now;

	public TagDetailModel SelectedTag { get; set; }

	public ObservableCollection<TagDetailModel> Tags { get; set; } = new();
	public ObservableCollection<ActivityListModel> Activities { get; set; } = new();
	
	public ActivityListViewModel(
		IMessengerService messengerService,
		IActivityFacade activityFacade,
		INavigationService navigationService,
		ISharedUserIdService userIdService,
		ITagFacade tagFacade,
		ISharedActivityIdService activityIdService,
		TagDetailModel selectedTag, IActivityTagFacade activityTagFacade) : base(messengerService)
	{
		_messengerService = messengerService;
		_activityFacade = activityFacade;
		_navigationService = navigationService;
		_userIdService = userIdService;
		_tagFacade = tagFacade;
		_activityIdService = activityIdService;
		SelectedTag = selectedTag;
		_activityTagFacade = activityTagFacade;
		_messengerService.Messenger.Register<NavigationMessage>(this);
		_messengerService.Messenger.Register<ActivityAddedMessage>(this);
		_messengerService.Messenger.Register<ActivityDeletedMessage>(this);
		_messengerService.Messenger.Register<LogOutMessage>(this);
		_messengerService.Messenger.Register<TagAddedMessage>(this);
	}

	private async Task LoadTagsAsync()
	{
		var tmpTags = await _tagFacade.GetAsyncUser(_userIdService.UserId);
		Tags = tmpTags.ToObservableCollection();
		Tags.Insert(0, TagDetailModel.Empty);
	}

	protected override async Task LoadDataAsync()
	{
		var tmpActivities = await _activityFacade.GetUserActivitiesAsync(_userIdService.UserId);
		var tmpUserTags = await _tagFacade.GetAsyncUser(_userIdService.UserId);
		var tmpActList = tmpActivities.ToList();
		var tmpUsTa = tmpUserTags.ToList();
		foreach (var activity in tmpActList)
		{
			activity.Tags.Clear();
			var tmpAtBindings = await _activityTagFacade.GetAsync(activity.Id);

			foreach (var tmpAtBinding in tmpAtBindings)
			{
				addTagToActivity(tmpAtBinding, tmpUsTa, activity);
			}
		}
		Activities = tmpActList.ToObservableCollection();
	}

	private void addTagToActivity(ActivityTagListEntity at, IEnumerable<TagDetailModel> userTags, ActivityListModel activity)
	{
		foreach (var userTag in userTags)
		{
			if (userTag.Id == at.TagId)
			{
				activity.Tags.Add(userTag);
			}
		}
	}

	[RelayCommand]
	private async void ApplyFilter()
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
			var tmpActivities = await _activityFacade.GetActivitiesDateTagFilterAsync(_userIdService.UserId, From, To, TagId);
			Activities = tmpActivities.ToObservableCollection();
		}
		else
		{
			MessageBox.Show("\"From\" musi být dříve, než \"To\"...",
				"Hupsík dupsík...", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	[RelayCommand]
	private async void ListAll()
	{
		await LoadDataAsync();
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
		_userIdService.UserId = Guid.Empty;
		_navigationService.NavigateTo<HomeViewModel>();
		_messengerService.Send(new LogOutMessage());
	}

	[RelayCommand]
	private void GoToCreateActivity()
	{
		_navigationService.NavigateTo<CreateActivityViewModel>();
		_messengerService.Send(new NavigationMessage());
	}

	[RelayCommand]
	private void GoToEditActivity(Guid activityId)
	{
		_activityIdService.ActivityId = activityId;
		_navigationService.NavigateTo<ActivityEditViewModel>();
		_messengerService.Send(new ActivityEditNavigationMessage());
	}
	
	public async void Receive(NavigationMessage message)
	{
		if (_firstLoad)
		{
			_firstLoad = false;
			await LoadTagsAsync();
			await LoadDataAsync();
		}
	}
	
	public void Receive(LogOutMessage message)
	{
		_firstLoad = true;
	}
	
	public async void Receive(ActivityAddedMessage message)
	{
		await LoadDataAsync();
	}
	
	public async void Receive(TagAddedMessage message)
	{
		await LoadTagsAsync();
	}

	public async void Receive(ActivityDeletedMessage message)
	{
		await LoadDataAsync();
	}
}