using System;
using System.Collections;
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

namespace WpfApp1.APP.ViewModels;

public partial class CreateActivityViewModel : ViewModelBase,
	IRecipient<NavigationMessage>,
	IRecipient<LogOutMessage>,
	IRecipient<TagAddedMessage>
{
	private ISharedUserIdService _idService;
	private IActivityFacade _activityFacade;
	private INavigationService _navigationService;
	private IMessengerService _messengerService;
	private IActivityTagFacade _activityTagFacade;
	private ITagFacade _tagFacade;
	private bool _firstLoad = true;

	public ObservableCollection<UserListModel> AvailableUsers { get; set; } = new();
	public IEnumerable<Guid> SelectedUsers { get; set; } = new List<Guid>();
	public ObservableCollection<TagDetailModel> AvailableTags { get; set; } = new();
	public TagDetailModel SelectedTag { get; set; } = TagDetailModel.Empty;

	public ActivityDetailModel Activity { get; set; } = new()
	{
		Name = String.Empty,
		DateTimeFrom = DateTime.Now,
		DateTimeTo = DateTime.Now,
		Color = default
	};


	public CreateActivityViewModel(
		IMessengerService messengerService,
		ISharedUserIdService idService,
		IActivityFacade activityFacade,
		INavigationService navigationService,
		IActivityTagFacade activityTagFacade,
		ITagFacade tagFacade)
		: base(messengerService)
	{
		_messengerService = messengerService;
		_idService = idService;
		_activityFacade = activityFacade;
		_navigationService = navigationService;
		_activityTagFacade = activityTagFacade;
		_tagFacade = tagFacade;
		_messengerService.Messenger.Register<NavigationMessage>(this);
		_messengerService.Messenger.Register<LogOutMessage>(this);
		_messengerService.Messenger.Register<TagAddedMessage>(this);
	}

	protected override async Task LoadDataAsync()
	{
		var tmp = await _tagFacade.GetAsyncUser(_idService.UserId);
		AvailableTags = tmp.ToObservableCollection();
	}

	[RelayCommand]
	private async Task CreateActivity()
	{
		SelectedUsers = SelectedUsers.Append(_idService.UserId);
		var tmpActivity = await _activityFacade.CreateActivityAsync(Activity, SelectedUsers);
		if (SelectedTag.Id != Guid.Empty)
		{
			await _activityTagFacade.SaveAsync(tmpActivity.Id, SelectedTag.Id);
		}
		Activity = ActivityDetailModel.Empty;
		_navigationService.NavigateTo<ActivityListViewModel>();
		_messengerService.Send(new ActivityAddedMessage());
	}

	[RelayCommand]
	private void GoToActivityList()
	{
		_navigationService.NavigateTo<ActivityListViewModel>();
	}

	public async void Receive(NavigationMessage message)
	{
		if (_firstLoad)
		{
			await LoadDataAsync();
			_firstLoad = false;
		}
	}

	public void Receive(LogOutMessage message)
	{
		_firstLoad = true;
	}

	public async void Receive(TagAddedMessage message)
	{
		await LoadDataAsync();
	}
}