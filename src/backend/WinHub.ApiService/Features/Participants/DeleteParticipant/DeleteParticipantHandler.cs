using MediatR;
using WinHub.ApiService.Database;
using WinHub.Shared.Common;

namespace WinHub.ApiService.Features.Participants.DeleteParticipant;

public class DeleteParticipantHandler(WinHubContext dbContext) : IRequestHandler<DeleteParticipantCommand, Result>
{
	public async Task<Result> Handle(DeleteParticipantCommand request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

		var participant = await dbContext
			.Participants.FindAsync([request.Id], cancellationToken)
			.ConfigureAwait(true);

		if (participant == null)
			return Result.Failure(new ApiError(
				"DeleteParticipant.Null",
				"The participant with the specified ID was not found"
			));

		dbContext.Remove(participant);

		await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

		return Result.Success(participant);
	}
}
