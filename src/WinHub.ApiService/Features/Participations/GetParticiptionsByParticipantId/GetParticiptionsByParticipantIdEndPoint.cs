using Carter;

using MediatR;

namespace WinHub.ApiService.Features.Participations.GetParticiptionsByParticipantId;

public class GetParticiptionsByParticipantIdEndPoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app) =>
		app.MapGet("api/participation{participantId}", async (Guid id, ISender sender) =>
		{
			var query = new GetParticiptionsByParticipantIdQuery { ParticipantId = id };

			var result = await sender.Send(query).ConfigureAwait(true);

			if (result.IsFailure)
				return Results.NotFound(result.Error);

			return Results.Ok(result);
		});
}
