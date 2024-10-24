namespace WinHub.ApiService.Entities;

public class Participation : BaseEntity<Guid>
{
    public Guid ContestId { get; set; }
    public Guid ParticipantId { get; set; }

    public Contest Contest { get; set; } = null!;
    public Participant Participant { get; set; } = null!;
}