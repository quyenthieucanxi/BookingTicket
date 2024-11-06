using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared;
using Domain.ValueObjects;
using MediatR;

namespace Application.Airports.Commands;

public sealed class CreateAirportCommandHandler : ICommandHandler<CreateAirportCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAirportRepository _airportRepository;

    public CreateAirportCommandHandler(
        IUnitOfWork unitOfWork, 
        IAirportRepository airportRepository)
    {
        _unitOfWork = unitOfWork;
        _airportRepository = airportRepository;
    }

    public async Task<Result> Handle(CreateAirportCommand request, CancellationToken cancellationToken)
    {
        var airportExist = await _airportRepository.FindAsync(a => a.Code == request.Code,cancellationToken);
        if (airportExist is not null)
        {
            return Result.Failure(DomainErrors.Airport.CodeAlreadyInUse);
        }
        var airPort = Airport.Create(AirportId.Create, request.Name, request.Location, request.Code);
        _airportRepository.Add(airPort);
        await _unitOfWork.SaveChanges(cancellationToken);
        return Result.Success();
    }
}