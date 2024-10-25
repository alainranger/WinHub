using MediatR;

using WinHub.ApiService.Common;

namespace WinHub.ApiService.Features.Participants.UpdateParticipant;

public record UpdateParticipantCommand : IRequest<Result>
{
	public Guid Id { get; set; }
	public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;	
}
