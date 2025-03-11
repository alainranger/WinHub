using Carter;

using Mapster;

using MediatR;

using WinHub.ApiService.Contracts.ParticipantFeature;

namespace WinHub.ApiService.Features.Participants.CreateParticipant;

public class CreateParticipantEndPoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app) =>
		app.MapPost("api/participant", async (CreateParticipantRequest request, ISender sender) =>
		{
			var command = request.Adapt<CreateParticipantCommand>();

			var result = await sender.Send(command).ConfigureAwait(true);

			if (result.IsFailure)
				return Results.BadRequest(result.Error);

			return Results.Ok(result.Value);
		});
}
