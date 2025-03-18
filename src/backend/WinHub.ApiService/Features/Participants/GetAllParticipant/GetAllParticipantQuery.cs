using MediatR;
using WinHub.ApiService.Contracts.ParticipantFeature;
using WinHub.Shared.Common;

namespace WinHub.ApiService.Features.Participants.GetAllParticipant;

public class GetAllParticipantQuery : IRequest<Result<List<ParticipantResponse>>>
{
}
