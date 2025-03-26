namespace WinHub.Blazor.Models;

internal class Participation : BaseEntity<Guid>
{
	public Guid ContestId { get; set; }
	public Guid ParticipantId { get; set; }

	public Contest Contest { get; set; } = null!;
	public Participant Participant { get; set; } = null!;
}
