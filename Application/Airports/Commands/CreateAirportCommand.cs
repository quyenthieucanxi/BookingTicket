using Application.Abstractions.Messaging;
using Domain.ValueObjects;

namespace Application.Airports.Commands;

public sealed record CreateAirportCommand(string Name, string Location, string Code) : ICommand;