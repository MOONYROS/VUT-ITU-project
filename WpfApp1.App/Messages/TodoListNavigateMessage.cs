using System;

namespace WpfApp1.App.Messages;

public record TodoListNavigateMessage
{
	public Guid UserGuid { get; set; }
}