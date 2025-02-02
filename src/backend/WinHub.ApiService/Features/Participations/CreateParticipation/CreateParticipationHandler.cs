using FluentValidation;
using MediatR;
using WinHub.ApiService.Common;
using WinHub.ApiService.Database;
using WinHub.ApiService.Entities;

namespace WinHub.ApiService.Features.Participations.CreateParticipation;

public class CreateParticipationHandler(WinHubContext dbContext) : IRequestHandler<CreateParticipationCommand, Result<Guid>>
{
	public async Task<Result<Guid>> Handle(CreateParticipationCommand request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);      
        
        var participation = new Participation()
        {
            Id = Guid.NewGuid(),
            ContestId = request.ContestId,
            ParticipantId = request.ParticipantId,
        };

        dbContext.Add(participation);

		await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

        return participation.Id;
	}
}