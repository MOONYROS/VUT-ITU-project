using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

public partial class ActivityEditViewModel : ViewModelBase,
	IRecipient<ActivityEditNavigationMessage>
{
	private readonly ISharedActivityIdService _activityIdService;
	private readonly ISharedUserIdService _userIdService;
	private INavigationService _navigationService;
	private IMessengerService _messengerService;
	private readonly IActivityFacade _activityFacade;
	private ITagFacade _tagFacade;
	private IActivityTagFacade _activityTagFacade;

	public ActivityDetailModel Activity { get; set; }
	public ObservableCollection<TagSelectModel> AvailableTagsSelect { get; set; } = new();
	public IEnumerable<Guid> AssignedTags { get; set; } = new List<Guid>();



	public ActivityEditViewModel(
		IMessengerService messengerService,
		ISharedActivityIdService activityIdService,
		ISharedUserIdService userIdService,
		INavigationService navigationService,
		IActivityFacade activityFacade, 
		ITagFacade tagFacade, 
		IActivityTagFacade activityTagFacade) :
		base(messengerService)
	{
		_messengerService = messengerService;
		_activityIdService = activityIdService;
		_userIdService = userIdService;
		_navigationService = navigationService;
		_activityFacade = activityFacade;
		_tagFacade = tagFacade;
		_activityTagFacade = activityTagFacade;
		_messengerService.Messenger.Register<ActivityEditNavigationMessage>(this);
	}

	protected override async Task LoadDataAsync()
	{
		AssignedTags = new List<Guid>();
		Activity = await _activityFacade.GetAsync(_activityIdService.ActivityId);
		var tags = Activity.Tags;
		foreach (var tag in tags)
		{
			AssignedTags = AssignedTags.Append(tag.Id);
		}
		
		//tags
		var tmp = await _tagFacade.GetAsyncUser(_userIdService.UserId);

		IEnumerable<TagSelectModel> idkbro2 = new List<TagSelectModel>();
		foreach (var tagDetail in tmp)
		{
			var tagSelect = TagSelectModel.Empty;
			tagSelect.Id = tagDetail.Id;
			tagSelect.Name = tagDetail.Name;
			tagSelect.Color = tagDetail.Color;
			
			//pokud uz je v aktivite
			if (AssignedTags.Contains(tagDetail.Id))
			{
				tagSelect.IsChecked = true;
			}
			
			idkbro2 = idkbro2.Append(tagSelect);
		}
		AvailableTagsSelect = idkbro2.ToObservableCollection();
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
	private async Task EditActivity()
	{
		if (!ValidCheck())
		{
			return;
		}
		await _activityFacade.SaveAsync(Activity);
		
		foreach (var tag in AvailableTagsSelect)
		{
			if (tag.IsChecked)
			{
				if (!AssignedTags.Contains(tag.Id))
				{
					await _activityTagFacade.SaveAsync(Activity.Id, tag.Id);
					tag.IsChecked = false;
				}
			}
			else //!tag.IsChecked
			{
				if (AssignedTags.Contains(tag.Id))
				{
					await _activityTagFacade.DeleteAsync(Activity.Id, tag.Id);
				}
			}
		}
		
		_navigationService.NavigateTo<ActivityListViewModel>();
		_messengerService.Send(new ActivityAddedMessage());
	}

	[RelayCommand]
	private void GoToActivityListView()
	{
		_navigationService.NavigateTo<ActivityListViewModel>();
	}
	
	[RelayCommand]
	private async Task DeleteActivity()
	{
		await _activityFacade.RemoveActivityFromUserAsync(Activity.Id, _userIdService.UserId);
		_navigationService.NavigateTo<ActivityListViewModel>();
		_messengerService.Send(new ActivityDeletedMessage());
	}

	public async void Receive(ActivityEditNavigationMessage message)
	{
		await LoadDataAsync();
	}
}