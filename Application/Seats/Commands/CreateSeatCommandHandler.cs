using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared;
using Domain.ValueObjects;

namespace Application.Seats.Commands;

public class CreateSeatCommandHandler: ICommandHandler<CreateSeatCommand>
{
    private readonly ISeatRepository _seatRepository;
    private readonly IFlightRepository _flightRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSeatCommandHandler(ISeatRepository seatRepository, 
        IUnitOfWork unitOfWork, IFlightRepository flightRepository)
    {
        _seatRepository = seatRepository;
        _unitOfWork = unitOfWork;
        _flightRepository = flightRepository;
    }

    public async Task<Result> Handle(CreateSeatCommand request, CancellationToken cancellationToken)
    {
        if (await _seatRepository.CheckSeatNumberExist(request.SeatNumber, cancellationToken))
        {
            return Result.Failure(DomainErrors.Seat.SeatNumberAlreadyInUse);
        }

        if (!await _seatRepository.CheckFlightExist(request.FlightId, cancellationToken))
        {
            return Result.Failure(DomainErrors.Seat.FlightNotFound);
        }

        var flight = await _flightRepository.GetByIdWithSeats(request.FlightId, cancellationToken);
        var seat = Seat.Create(SeatId.Create(), request.SeatNumber, 
            request.Class, request.IsAvailable, request.FlightId);
        flight!.AddSeat(seat);
        _flightRepository.Update(flight);
        await _unitOfWork.SaveChanges(cancellationToken);
        return Result.Success();
    }
}