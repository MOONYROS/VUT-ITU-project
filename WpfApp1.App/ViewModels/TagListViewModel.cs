using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using WpfApp1.App;
using WpfApp1.App.Messages;
using WpfApp1.APP.Services.Interfaces;
using WpfApp1.BL.Facades.Interfaces;
using WpfApp1.BL.Models;

namespace WpfApp1.APP.ViewModels;

public partial class TagListViewModel : ViewModelBase,
	IRecipient<NavigationMessage>,
	IRecipient<TagAddedMessage>,
	IRecipient<LogOutMessage>
{
	private readonly ITagFacade _tagFacade;
	private readonly IMessengerService _messengerService;
	private readonly INavigationService _navigationService;
	private ISharedUserIdService _idService;
	private bool _firstLoad = true;

	public ObservableCollection<TagDetailModel> Tags { get; set; } = new();
		public TagListViewModel(
			IMessengerService messengerService, 
			ITagFacade tagFacade, 
			INavigationService navigationService, 
			ISharedUserIdService idService) : base(messengerService)
		{
			_messengerService = messengerService;
			_tagFacade = tagFacade;
			_navigationService = navigationService;
			_idService = idService;
			_messengerService.Messenger.Register<NavigationMessage>(this);
			_messengerService.Messenger.Register<TagAddedMessage>(this);
			_messengerService.Messenger.Register<LogOutMessage>(this);
		}

		protected override async Task LoadDataAsync()
		{
			var bruh = await _tagFacade.GetAsyncUser(_idService.UserId);
			Tags = bruh.ToObservableCollection();
		}
		
		[RelayCommand]
		private async void test()
		{
			// MessageBox.Show($"{_idService.UserId}", "jolol", MessageBoxButton.OK, MessageBoxImage.Error);
			var tag = TagDetailModel.Empty;
			tag.Name = "Kokot";
			tag.Color = Color.Fuchsia;
			await _tagFacade.SaveAsync(tag, _idService.UserId);
			await LoadDataAsync();
		}
		
		[RelayCommand]
		private void GoToTagListView()
		{
			_navigationService.NavigateTo<TodoListViewModel>();
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

		public async void Receive(TagAddedMessage message)
		{
			await LoadDataAsync();
		}

		public void Receive(LogOutMessage message)
		{
			_firstLoad = true;
		}
}