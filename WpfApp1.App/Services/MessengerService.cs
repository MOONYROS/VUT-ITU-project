using CommunityToolkit.Mvvm.Messaging;
using WpfApp1.APP.Services.Interfaces;

namespace WpfApp1.APP.Services;

public class MessengerService : IMessengerService
{
	public IMessenger Messenger { get; }

	public MessengerService(IMessenger messenger)
	{
		Messenger = messenger;
	}

	public void Send<TMessage>(TMessage message)
		where TMessage : class
	{
		Messenger.Send(message);
	}
}