namespace Application.Seats.Commands;

public record CreateSeatRequest(string SeatNumber,string Class,
    bool IsAvailable,Guid FlightId);