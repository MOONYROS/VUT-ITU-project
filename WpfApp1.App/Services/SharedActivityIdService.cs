using System;
using WpfApp1.APP.Services.Interfaces;

namespace WpfApp1.APP.Services;

public class SharedActivityIdService : ISharedActivityIdService
{
	public Guid ActivityId { get; set; } = Guid.Empty;
}