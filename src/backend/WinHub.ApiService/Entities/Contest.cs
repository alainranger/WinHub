namespace WinHub.ApiService.Entities;

public class Contest : BaseEntity<Guid>
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public DateTime StartDateTime { get; set; }
	public DateTime EndDateTime { get; set; }
	public DateTime ContestDateTime { get; set; }

	public ICollection<Participation> Participations { get; } = [];
}
