namespace WinHub.ApiService.Entities;

public class Participant : BaseEntity<Guid>
{
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public ICollection<Participation> Participations { get;} = [];
}