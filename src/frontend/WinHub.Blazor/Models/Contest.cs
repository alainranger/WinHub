using System.ComponentModel.DataAnnotations;
using WinHub.Blazor.DataAnotations;

namespace WinHub.Blazor.Models;

internal sealed class Contest : BaseEntity<Guid>
{
	[Required]
	public string Name { get; set; } = string.Empty;

	public string Description { get; set; } = string.Empty;

	[Required]
	[DateLowerThan(nameof(EndDateTime))]
	public DateTime? StartDateTime { get; set; }

	[Required]
	[DateGreaterThan(nameof(StartDateTime))]
	public DateTime? EndDateTime { get; set; }

	[Required]
	[DateGreaterThan(nameof(EndDateTime))]
	public DateTime? ContestDateTime { get; set; }

	public ICollection<Participation> Participations { get; } = [];
}
