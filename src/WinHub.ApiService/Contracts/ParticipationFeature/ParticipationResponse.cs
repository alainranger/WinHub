namespace WinHub.ApiService.Contracts.ParticipationFeature;

public class ParticipationResponse
{
    public Guid Id { get; set; }
    public Guid ContestId { get; set; }
    public Guid ParticipantId { get; set; }
}