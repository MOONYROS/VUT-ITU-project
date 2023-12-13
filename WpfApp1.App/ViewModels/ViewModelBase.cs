using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using WpfApp1.APP.Services.Interfaces;
using WpfApp1.APP.ViewModels.Interfaces;

namespace WpfApp1.APP.ViewModels;

public abstract class ViewModelBase : ObservableRecipient, IViewModel
{
	private bool _isRefreshRequired = true;
	protected readonly IMessengerService MessengerService;

	protected ViewModelBase(IMessengerService messengerService)
		: base(messengerService.Messenger)
	{
		this.MessengerService = messengerService;
		IsActive = true;
	}

	public async Task OnAppearingAsync()
	{
		if (_isRefreshRequired)
		{
			await LoadDataAsync();

			_isRefreshRequired = false;
		}
	}

	protected virtual Task LoadDataAsync()
		=> Task.CompletedTask;
}