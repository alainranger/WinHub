namespace WinHub.ApiService.Contracts.ParticipantFeature;

public class ParticipantResponse
{
    public Guid Id { get; set;}
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}