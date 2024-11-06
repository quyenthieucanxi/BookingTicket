using Application.Abstractions.Messaging;
using Domain.ValueObjects;

namespace Application.Passengers.Commands;

public record CreatePassengerCommand(string Name,string PassportNumber,
    string Nationality,DateTime DateOfBirth, BookingId BookingId) : ICommand;