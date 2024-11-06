using Application.Abstractions.Messaging;
using Domain.ValueObjects;

namespace Application.Airlines.Queries;

public sealed record GetAirlineByIdQuery(AirlineId Id) : IQuery<AirlineResponse>;