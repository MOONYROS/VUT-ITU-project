using System;
using WpfApp1.APP.Services.Interfaces;

namespace WpfApp1.APP.Services;

public class SharedUserIdService : ISharedUserIdService
{
	public Guid UserId { get; set; } = Guid.Empty;
}