using MediatR;

using WinHub.ApiService.Common;

namespace WinHub.ApiService.Features.Participants.CreateParticipant;

public record CreateParticipantCommand : IRequest<Result<Guid>>
{
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}