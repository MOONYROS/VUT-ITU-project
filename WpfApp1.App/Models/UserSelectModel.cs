using System;

namespace WpfApp1.App.Models;

public record UserSelectModel
{
	public required Guid Id { get; set; }
	public required string UserName { get; set; }
	public string? ImageUrl { get; set; }
	public bool IsChecked { get; set; }
	public static UserSelectModel Empty => new()
	{
		Id = Guid.Empty,
		UserName = string.Empty,
		IsChecked = false
	};
}