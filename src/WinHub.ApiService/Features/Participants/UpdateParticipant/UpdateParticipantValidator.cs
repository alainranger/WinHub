using FluentValidation;

namespace WinHub.ApiService.Features.Participants.UpdateParticipant;

public class UpdateParticipantValidator : AbstractValidator<UpdateParticipantCommand>
{
	public UpdateParticipantValidator()
	{
		RuleFor(c => c.Firstname).NotEmpty();
		RuleFor(c => c.Lastname).NotEmpty();
		RuleFor(c => c.Email).EmailAddress().NotEmpty();
	}
}
