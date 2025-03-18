using FluentValidation;
using MediatR;
using WinHub.ApiService.Database;
using WinHub.Shared.Common;

namespace WinHub.ApiService.Features.Participants.UpdateParticipant;

public class UpdateParticipantHandler(WinHubContext dbContext, IValidator<UpdateParticipantCommand> validator) : IRequestHandler<UpdateParticipantCommand, Result>
{
	public async Task<Result> Handle(UpdateParticipantCommand request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

		var validatorResult = await validator.ValidateAsync(request, cancellationToken).ConfigureAwait(false);
		if (!validatorResult.IsValid)
			return Result.Failure<Guid>(new ApiError(
				"UpdateParticipant.Validation",
				validatorResult.ToString()
			));

		var participant = await dbContext
				.Participants.FindAsync([request.Id], cancellationToken)
				.ConfigureAwait(true);

		if (participant == null)
			return Result.Failure(new ApiError(
				"UpdateParticipant.Null",
				"The participant with the specified ID was not found"
			));

		participant.Id = request.Id;
		participant.Firstname = request.Firstname;
		participant.Lastname = request.Lastname;
		participant.Email = request.Email;

		dbContext.Update(participant);

		await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

		return Result.Success(participant);
	}
}
