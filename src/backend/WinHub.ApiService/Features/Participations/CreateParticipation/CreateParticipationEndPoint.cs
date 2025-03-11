using Carter;

using Mapster;

using MediatR;

using WinHub.ApiService.Contracts.ParticipationFeature;

namespace WinHub.ApiService.Features.Participations.CreateParticipation;

public class CreateParticipationEndPoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app) =>
		app.MapPost("api/participation", async (CreateParticipationRequest request, ISender sender) =>
		{
			var command = request.Adapt<CreateParticipationCommand>();

			var result = await sender.Send(command).ConfigureAwait(true);

			if (result.IsFailure)
				return Results.BadRequest(result.Error);

			return Results.Ok(result.Value);
		});
}
