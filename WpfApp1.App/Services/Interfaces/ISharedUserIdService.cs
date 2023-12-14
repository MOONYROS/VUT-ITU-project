using System;

namespace WpfApp1.APP.Services.Interfaces;

public interface ISharedUserIdService
{
	Guid UserId { get; set; }
}