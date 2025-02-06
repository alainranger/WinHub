using FluentValidation;

using MediatR;

using WinHub.ApiService.Common;
using WinHub.ApiService.Database;
using WinHub.ApiService.Entities;

namespace WinHub.ApiService.Features.Participants.CreateParticipant;

public class CreateParticipantHandler(WinHubContext dbContext, IValidator<CreateParticipantCommand> validator) : IRequestHandler<CreateParticipantCommand, Result<Guid>>
{
	public async Task<Result<Guid>> Handle(CreateParticipantCommand request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

        var validatorResult = await validator.ValidateAsync(request, cancellationToken).ConfigureAwait(false);
		if (!validatorResult.IsValid)
			return Result.Failure<Guid>(new ApiError(
				"CreateParticipant.Validation",
				validatorResult.ToString()
			));

		var participant = new Participant()
		{
			Id = Guid.NewGuid(),
			Firstname = request.Firstname,
            Lastname = request.Lastname,
            Email = request.Email,
            CreatedAt = DateTime.UtcNow        
		};

		dbContext.Add(participant);

		await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

        return participant.Id;
	}
}