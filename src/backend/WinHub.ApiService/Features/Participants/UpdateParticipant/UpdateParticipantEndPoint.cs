using Carter;

using Mapster;

using MediatR;

using WinHub.ApiService.Contracts.ParticipantFeature;

namespace WinHub.ApiService.Features.Participants.UpdateParticipant;

public class UpdateParticipantEndPoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app) =>
		app.MapPut("api/participant", async (Guid id, UpdateParticipantRequest request, ISender sender) =>
		{
			var command = request.Adapt<UpdateParticipantCommand>();

			var result = await sender.Send(command).ConfigureAwait(true);

			if (result.IsFailure)
				return Results.BadRequest(result.Error);

			return Results.Ok(result);
		});
}
