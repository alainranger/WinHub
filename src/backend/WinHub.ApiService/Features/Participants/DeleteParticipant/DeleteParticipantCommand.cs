using MediatR;

using WinHub.Shared.Common;

namespace WinHub.ApiService.Features.Participants.DeleteParticipant;

public record DeleteParticipantCommand : IRequest<Result>
{
	public Guid Id { get; set; }
}
