using Carter;

using Mapster;

using MediatR;

using WinHub.ApiService.Contracts;

namespace WinHub.ApiService.Features.Contests.CreateContest;

public class CreateContestEndPoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app) =>
		app.MapPost("api/contest", async (CreateContestRequest request, ISender sender) =>
		{
			var command = request.Adapt<CreateContestCommand>();

			var result = await sender.Send(command).ConfigureAwait(true);

			if (result.IsFailure)
				return Results.BadRequest(result.Error);

			return Results.Ok(result.Value);
		});
}
