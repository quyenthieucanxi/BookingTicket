using Application.Abstractions.Messaging;
using Domain.ValueObjects;

namespace Application.Airports.Queries;

public sealed record GetAirportByIdQuery(AirportId Id) : IQuery<AirportResponse>;