namespace Application.Passengers.Commands;

public record CreatePassengerRequest(string Name,string PassportNumber,
    string Nationality,DateTime DateOfBirth);