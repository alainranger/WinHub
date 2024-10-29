using MediatR;
using WinHub.ApiService.Common;

namespace WinHub.ApiService.Features.Participations.CreateParticipation;

public class CreateParticipationCommand : IRequest<Result<Guid>>
{
    public Guid ContestId { get; set; }
    public Guid ParticipantId { get; set; }
}