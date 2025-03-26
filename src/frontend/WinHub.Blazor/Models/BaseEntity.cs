﻿namespace WinHub.Blazor.Models;

internal class BaseEntity<TKey>
{
	public TKey? Id { get; set; }
	public DateTime? CreatedAt { get; set; }
	public string CreatedBy { get; set; } = string.Empty;
	public DateTime? UpdatedAt { get; set; }
	public string UpdatedBy { get; set; } = string.Empty;
}
