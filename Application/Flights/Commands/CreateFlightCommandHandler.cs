using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared;
using Domain.ValueObjects;

namespace Application.Flights.Commands;

public sealed class CreateFlightCommandHandler: ICommandHandler<CreateFlightCommand>
{
    private readonly IFlightRepository _flightRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateFlightCommandHandler(IUnitOfWork unitOfWork, IFlightRepository flightRepository)
    {
        _unitOfWork = unitOfWork;
        _flightRepository = flightRepository;
    }

    public async Task<Result> Handle(CreateFlightCommand request, CancellationToken cancellationToken)
    {
        // Check flight has  already FlightNumber in use
        if (await _flightRepository.CheckFlightNumberExist(request.FlightNumber,cancellationToken))
        {
            return Result.Failure(DomainErrors.Flight.FlightNumverAlreadyInUse);
        }
        //Check flight has already OriginAirport in  use
        if ( !await _flightRepository.CheckOriginAirportExist(request.OriginAirportId,cancellationToken))
        {
            return Result.Failure(DomainErrors.Flight.OriginAirportNotFound);
        }
        
        //Check flight has already DestinationAirport in  use
        
        if (!await _flightRepository.CheckDestinationAirportExist(request.DestinationAirportId,cancellationToken))
        {
            return Result.Failure(DomainErrors.Flight.DestinationAirportNotFound);
        }
        //Check flight has already Airport in  use
        if (!await _flightRepository.CheckAirlineExist(request.AirlineId,cancellationToken))
        {
            return Result.Failure(DomainErrors.Flight.AirlineNotFound);
        }
        // Create Flight
        var flight = Flight.Create(FlightId.Create(), request.FlightNumber, request.DepartureTime,
            request.ArrivalTime, request.Price, request.Duration, request.OriginAirportId,
            request.DestinationAirportId, request.AirlineId);
        //Add
        _flightRepository.Add(flight);
        await _unitOfWork.SaveChanges(cancellationToken);
        return Result.Success();
    }

   
}