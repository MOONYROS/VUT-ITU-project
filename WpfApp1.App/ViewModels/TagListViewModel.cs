using System.Collections.ObjectModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using WpfApp1.App;
using WpfApp1.App.Messages;
using WpfApp1.APP.Services.Interfaces;
using WpfApp1.BL.Facades.Interfaces;
using WpfApp1.BL.Models;

namespace WpfApp1.APP.ViewModels;

public partial class TagListViewModel : ViewModelBase,
	IRecipient<NavigationMessage>
{
	private readonly ITagFacade _tagFacade;
	private readonly IMessengerService _messengerService;
	private readonly INavigationService _navigationService;
	private ISharedUserIdService _idService;

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
			_messengerService.Send(new NavigationMessage());
			_navigationService.NavigateTo<TodoListViewModel>();
		}
		
		public async void Receive(NavigationMessage message)
		{
			await LoadDataAsync();
		}
}