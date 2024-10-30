namespace WinHub.ApiService.Contracts.ParticipationFeature;

public class CreateParticipationRequest
{
     public Guid ContestId { get; set; }
    public Guid ParticipantId { get; set; }
}