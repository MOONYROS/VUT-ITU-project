﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using WpfApp1.App;
using WpfApp1.App.Messages;
using WpfApp1.App.Models;
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
	private IUserFacade _userFacade;
	private bool _firstLoad = true;

	public ObservableCollection<UserSelectModel> AvailableUsers { get; set; } = new();
	public ObservableCollection<TagSelectModel> AvailableTagsSelect { get; set; } = new();
	public IEnumerable<Guid> SelectedUsers { get; set; } = new List<Guid>();
	public IEnumerable<Guid> SelectedTags { get; set; } = new List<Guid>();

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
		ITagFacade tagFacade, 
		IUserFacade userFacade)
		: base(messengerService)
	{
		_messengerService = messengerService;
		_idService = idService;
		_activityFacade = activityFacade;
		_navigationService = navigationService;
		_activityTagFacade = activityTagFacade;
		_tagFacade = tagFacade;
		_userFacade = userFacade;
		_messengerService.Messenger.Register<NavigationMessage>(this);
		_messengerService.Messenger.Register<LogOutMessage>(this);
		_messengerService.Messenger.Register<TagAddedMessage>(this);
	}

	protected override async Task LoadDataAsync()
	{
		//tags
		var tmp = await _tagFacade.GetAsyncUser(_idService.UserId);

		IEnumerable<TagSelectModel> idkbro2 = new List<TagSelectModel>();
		foreach (var tagDetail in tmp)
		{
			var tagSelect = TagSelectModel.Empty;
			tagSelect.Id = tagDetail.Id;
			tagSelect.Name = tagDetail.Name;
			tagSelect.Color = tagDetail.Color;
			
			idkbro2 = idkbro2.Append(tagSelect);
		}
		AvailableTagsSelect = idkbro2.ToObservableCollection();
		
		//users
		var tmp2 = await _userFacade.GetAsync();
		IEnumerable<UserSelectModel> idkbro = new List<UserSelectModel>();
		foreach (var userDetail in tmp2)
		{
			if (userDetail.Id != _idService.UserId)
			{
				var userSelect = UserSelectModel.Empty;
				userSelect.Id = userDetail.Id;
				userSelect.UserName = userDetail.UserName;
				userSelect.ImageUrl = userDetail.ImageUrl;
				
				idkbro = idkbro.Append(userSelect);
			}
		}
		AvailableUsers = idkbro.ToObservableCollection();
	}

	private bool ValidCheck()
	{
		if (String.IsNullOrWhiteSpace(Activity.Name))
		{
			MessageBox.Show("Pole pro jméno aktivity nesmí být prazdné",
				"Hupsík dupsík...", MessageBoxButton.OK, MessageBoxImage.Error);
			return false;
		}
		if ((DateTime.Compare(Activity.DateTimeFrom, Activity.DateTimeTo) >= 0))
		{
			MessageBox.Show("\"From\" musi být dříve, než \"To\"...",
				"Hupsík dupsík...", MessageBoxButton.OK, MessageBoxImage.Error);
			return false;
		}
		return true;
	}

	[RelayCommand]
	private async Task CreateActivity()
	{
		if (!ValidCheck())
		{
			return;
		}
		SelectedUsers = SelectedUsers.Append(_idService.UserId);
		foreach (var user in AvailableUsers)
		{
			if (user.IsChecked)
			{
				SelectedUsers = SelectedUsers.Append(user.Id);
				user.IsChecked = false;
			}
		}
		var tmpActivity = await _activityFacade.CreateActivityAsync(Activity, SelectedUsers);
		SelectedUsers = new List<Guid>();

		foreach (var tag in AvailableTagsSelect)
		{
			if (tag.IsChecked)
			{
				await _activityTagFacade.SaveAsync(tmpActivity.Id, tag.Id);
				tag.IsChecked = false;
			}
		}
		
		Activity = new ActivityDetailModel
		{
			Name = String.Empty,
			DateTimeFrom = DateTime.Now,
			DateTimeTo = DateTime.Now,
			Color = default
		};
		_navigationService.NavigateTo<ActivityListViewModel>();
		_messengerService.Send(new ActivityAddedMessage());
	}

	[RelayCommand]
	private void GoToActivityList()
	{
		Activity = new ActivityDetailModel
		{
			Name = String.Empty,
			DateTimeFrom = DateTime.Now,
			DateTimeTo = DateTime.Now,
			Color = default
		};
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