using Application.Abstractions.Messaging;
using Domain.ValueObjects;

namespace Application.Seats.Commands;

public sealed record CreateSeatCommand(string SeatNumber,string Class,
    bool IsAvailable,FlightId FlightId) : ICommand;