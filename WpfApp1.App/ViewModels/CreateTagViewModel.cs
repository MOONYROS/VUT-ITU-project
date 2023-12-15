using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using WpfApp1.App.Messages;
using WpfApp1.APP.Services.Interfaces;
using WpfApp1.BL.Facades.Interfaces;
using WpfApp1.BL.Models;

namespace WpfApp1.APP.ViewModels;

public partial class CreateTagViewModel : ViewModelBase
{
	private readonly INavigationService _navigationService;
	private readonly ITagFacade _tagFacade;
	private readonly IMessengerService _messengerService;
	private ISharedUserIdService _idService;

	public TagDetailModel Tag { get; set; } = TagDetailModel.Empty;

	public CreateTagViewModel(
		INavigationService navigationService, 
		ITagFacade tagFacade, 
		IMessengerService messengerService, 
		ISharedUserIdService idService) : base(messengerService)
	{
		_navigationService = navigationService;
		_tagFacade = tagFacade;
		_messengerService = messengerService;
		_idService = idService;
	}
	
	[RelayCommand]
	private async Task CreateTag()
	{
		if (Tag.Name.Length < 1 || Tag.Name.Length > 9)
		{
			MessageBox.Show("Jmeno Tagu musí být dlouhé 1 až 9 znaků", "Hupsík dupsík...", MessageBoxButton.OK, MessageBoxImage.Error);
		}
		else
		{
			await _tagFacade.SaveAsync(Tag, _idService.UserId);
			Tag = TagDetailModel.Empty;
			_navigationService.NavigateTo<TagListViewModel>();
			_messengerService.Send(new TagAddedMessage());
		}
	}
	
	[RelayCommand]
	private void GoToTagListView()
	{
		Tag = TagDetailModel.Empty;
		_navigationService.NavigateTo<TagListViewModel>();
		_messengerService.Send(new NavigationMessage());
	}
}