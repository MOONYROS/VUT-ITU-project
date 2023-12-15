using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using WpfApp1.App.Messages;
using WpfApp1.APP.Services.Interfaces;
using WpfApp1.BL.Facades.Interfaces;
using WpfApp1.BL.Models;

namespace WpfApp1.APP.ViewModels;

public partial class CreateActivityViewModel : ViewModelBase
{
	private ISharedUserIdService _idService;
	private IActivityFacade _activityFacade;
	private INavigationService _navigationService;
	private IMessengerService _messengerService;
	private IActivityTagFacade _activityTagFacade;

	public IEnumerable<UserListModel> AvailableUserts { get; set; } = new List<UserListModel>();
	public IEnumerable<Guid> SelectedUsers { get; set; } = new List<Guid>();
	public IEnumerable<TagDetailModel> AvailableTags { get; set; } = new List<TagDetailModel>();
	public IEnumerable<Guid> SelectedTags { get; set; } = new List<Guid>();
	public ActivityDetailModel Activity { get; set; } = ActivityDetailModel.Empty;

	public CreateActivityViewModel(
		IMessengerService messengerService,
		ISharedUserIdService idService,
		IActivityFacade activityFacade,
		INavigationService navigationService,
		IActivityTagFacade activityTagFacade)
		: base(messengerService)
	{
		_messengerService = messengerService;
		_idService = idService;
		_activityFacade = activityFacade;
		_navigationService = navigationService;
		_activityTagFacade = activityTagFacade;
	}

	[RelayCommand]
	private async Task CreateActivity()
	{
		SelectedUsers.Append(_idService.UserId);
		await _activityFacade.CreateActivityAsync(Activity, SelectedUsers);
		foreach (var tagId in SelectedTags)
		{
			await _activityTagFacade.SaveAsync(Activity.Id, tagId);
		}
		_navigationService.NavigateTo<ActivityListViewModel>();
		_messengerService.Send(new ActivityAddedMessage());
	}
}