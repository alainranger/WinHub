using MediatR;

using WinHub.ApiService.Common;
using WinHub.ApiService.Contracts.ParticipantFeature;

namespace WinHub.ApiService.Features.Participants.GetAllParticipant;

public class GetAllParticipantQuery : IRequest<Result<List<ParticipantResponse>>>
{
}
