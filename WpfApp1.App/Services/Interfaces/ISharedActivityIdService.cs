using System;

namespace WpfApp1.APP.Services.Interfaces;

public interface ISharedActivityIdService
{
	Guid ActivityId { get; set; }
}