using Carter;

using Mapster;

using MediatR;

using WinHub.ApiService.Contracts;

namespace WinHub.ApiService.Features.Contests.UpdateContest;

public class UpdateContestEndPoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app) =>
		app.MapPut("api/contest", async (CreateContestRequest request, ISender sender) =>
		{
			var command = request.Adapt<UpdateContestCommand>();

			var result = await sender.Send(command).ConfigureAwait(false);

			if (result.IsFailure)
				return Results.BadRequest(result.Error);

			return Results.Ok(result.Value);
		});
}
