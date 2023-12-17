using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using WpfApp1.App;
using WpfApp1.App.Messages;
using WpfApp1.APP.Services.Interfaces;
using WpfApp1.BL.Facades.Interfaces;
using WpfApp1.BL.Models;
using WpfApp1.DAL.Entities;

namespace WpfApp1.APP.ViewModels;

public partial class ActivityCalendarViewModel : ViewModelBase,
	IRecipient<NavigationMessage>,
	IRecipient<LogOutMessage>,
	IRecipient<TagAddedMessage>,
	IRecipient<ActivityAddedMessage>,
	IRecipient<ActivityDeletedMessage>
{
	private readonly IActivityFacade _activityFacade;
	private readonly INavigationService _navigationService;
	private readonly ISharedUserIdService _idService;
	private readonly IMessengerService _messengerService;
	private readonly IActivityTagFacade _activityTagFacade;
	private readonly ITagFacade _tagFacade;
	private bool _firstLoad = true;

	public ObservableCollection<ActivityListModel> Activities { get; set; } = new();
	public DateTime Date { get; set; } = DateTime.Today;
	public TagDetailModel SelectedTag { get; set; }
	public ObservableCollection<TagDetailModel> Tags { get; set; }

	public ActivityCalendarViewModel(
		IActivityFacade activityFacade,
		INavigationService navigationService,
		ISharedUserIdService idService,
		IMessengerService messengerService,
		IActivityTagFacade activityTagFacade,
		TagDetailModel defaultTag,
		ITagFacade tagFacade)
	{
		_activityFacade = activityFacade;
		_navigationService = navigationService;
		_idService = idService;
		_messengerService = messengerService;
		_activityTagFacade = activityTagFacade;
		_tagFacade = tagFacade;
		SelectedTag = defaultTag;
		_messengerService.Messenger.Register<NavigationMessage>(this);
		_messengerService.Messenger.Register<ActivityAddedMessage>(this);
		_messengerService.Messenger.Register<ActivityDeletedMessage>(this);
		_messengerService.Messenger.Register<LogOutMessage>(this);
		_messengerService.Messenger.Register<TagAddedMessage>(this);
	}

	protected override async Task LoadDataAsync()
	{
		var tmpActivities2 = await _activityFacade.GetUserActivitiesAsync(_idService.UserId);
		var activitiesList2 = await FixTags(tmpActivities2);
		Activities = activitiesList2.ToObservableCollection();
	}

	private async Task LoadTagsAsync()
	{
		var tmpTags = await _tagFacade.GetAsyncUser(_idService.UserId);
		Tags = tmpTags.ToObservableCollection();
		Tags.Insert(0, TagDetailModel.Empty);
	}

	private async Task<List<ActivityListModel>> FixTags(IEnumerable<ActivityListModel> tmpActivities)
	{
		var tmpActList = tmpActivities.ToList();
		var tmpUserTags = await _tagFacade.GetAsyncUser(_idService.UserId);
		// Musi byt list, u IEnumerable se clear nepropise do puvodni kolekces
		tmpUserTags = tmpUserTags.ToList();
		foreach (var activity in tmpActList)
		{
			activity.Tags.Clear();
			var tmpAtBindings = await _activityTagFacade.GetAsync(activity.Id);

			foreach (var tmpAtBinding in tmpAtBindings)
			{
				AddTagToActivity(tmpAtBinding, tmpUserTags, activity);
			}
		}

		return tmpActList;
	}

	private void AddTagToActivity(ActivityTagListEntity at, IEnumerable<TagDetailModel> userTags, ActivityListModel activity)
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
	private void GoToTodoListView()
	{
		_navigationService.NavigateTo<TodoListViewModel>();
	}

	[RelayCommand]
	private void GoToTagListView()
	{
		_navigationService.NavigateTo<TagListViewModel>();
		_messengerService.Send(new NavigationMessage());
	}

	[RelayCommand]
	private void LogOut()
	{
		_idService.UserId = Guid.Empty;
		_navigationService.NavigateTo<HomeViewModel>();
		_messengerService.Send(new LogOutMessage());
	}

	[RelayCommand]
	private void GoToEditUserView()
	{
		_navigationService.NavigateTo<EditUserViewModel>();
		_messengerService.Send(new NavigationMessage());
	}

	[RelayCommand]
	private void GoToCreateActivity()
	{
		_navigationService.NavigateTo<CreateActivityViewModel>();
		_messengerService.Send(new NavigationMessage());
	}

	[RelayCommand]
	private void GoToActivityListView()
	{
		_navigationService.NavigateTo<ActivityListViewModel>();
	}

	[RelayCommand]
	private async Task ApplyFilter()
	{
		Guid? tagId;
		try
		{
			if (SelectedTag.Name == String.Empty)
			{
				tagId = null;
			}
			else
			{
				tagId = SelectedTag.Id;
			}
		}
		catch (Exception)
		{
			tagId = null;
		}

		var tmpActivities = await _activityFacade.GetActivitiesDateTagFilterAsync(_idService.UserId, Date, null, tagId);
		var activitiesList = await FixTags(tmpActivities);
		Activities = activitiesList.ToObservableCollection();
	}

	[RelayCommand]
	private async Task ListAll()
	{
		await LoadDataAsync();
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

	public async void Receive(TagAddedMessage message)
	{
		await LoadTagsAsync();
	}

	public async void Receive(ActivityAddedMessage message)
	{
		await LoadTagsAsync();
	}

	public async void Receive(ActivityDeletedMessage message)
	{
		await LoadDataAsync();
	}
}