using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared;
using Domain.ValueObjects;

namespace Application.Airlines.Commands;

public sealed class CreateAirlineCommandHandler : ICommandHandler<CreateAirlineCommand>
{
    private readonly IAirlineRepository _airlineRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAirlineCommandHandler(IAirlineRepository airlineRepository, IUnitOfWork unitOfWork)
    {
        _airlineRepository = airlineRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CreateAirlineCommand request, CancellationToken cancellationToken)
    {
        var airlineExist = await _airlineRepository.FindAsync(a => a.IATACode == request.IATACode,
            cancellationToken).ConfigureAwait(false);
        if (airlineExist is not null)
        {
            return Result.Failure(DomainErrors.Airline.IATACodeAlreadyInUse);
        }

        var airline = Airline.Create(AirlineId.Create(), request.Name, request.Country, request.IATACode);
        _airlineRepository.Add(airline);
        await _unitOfWork.SaveChanges(cancellationToken);
        return Result.Success();
    }
}