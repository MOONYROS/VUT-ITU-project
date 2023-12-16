using System;
using System.Drawing;

namespace WpfApp1.App.Models;

public record TagSelectModel
{
	public required Guid Id { get; set; }
	public required string Name { get; set; }
	public required Color Color { get; set; }
	public bool IsChecked { get; set; }
        
	public static TagSelectModel Empty => new()
	{
		Id = Guid.Empty,
		Name = string.Empty,
		Color = Color.Black,
		IsChecked = false
	};
}