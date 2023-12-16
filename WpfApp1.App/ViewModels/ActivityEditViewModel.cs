using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using WpfApp1.App.Messages;
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

	public ActivityDetailModel Activity { get; set; }

	public ActivityEditViewModel(
		IMessengerService messengerService,
		ISharedActivityIdService activityIdService,
		ISharedUserIdService userIdService,
		INavigationService navigationService,
		IActivityFacade activityFacade) :
		base(messengerService)
	{
		_messengerService = messengerService;
		_activityIdService = activityIdService;
		_userIdService = userIdService;
		_navigationService = navigationService;
		_activityFacade = activityFacade;
		_messengerService.Messenger.Register<ActivityEditNavigationMessage>(this);
	}

	protected override async Task LoadDataAsync()
	{
		Activity = await _activityFacade.GetAsync(_activityIdService.ActivityId);
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