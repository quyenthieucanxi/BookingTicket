using Domain.Shared;

namespace Domain.Errors;

public static class DomainErrors
{


    public static class User
    {
        public static readonly Error EmailAlreadyInUse = new(
            "User.EmailAlreadyInUse",
            "The specified email is already in use");

        public static readonly Error PasswordInvalid = new Error(
            "User.PasswordInValid",
            "Password must have least 6 characters,one non alphanumeric character, one digit ('0'-'9'), one uppercase, one lowercase");

        public static readonly Error NotFound = new Error("User.NotFound", "User is Not Found");
        
        public static readonly Error EmailNotFound = new Error("User.EmailNotFound", "The Email is Not Found");
        
        public static readonly Error PasswordWrong = new Error("User.PasswordWrong", "The Password is Wrong");
    }
    public static class Name
    {
        public static readonly Error TooLong =
            new Error(
                "Name.TooLong",
                "Name is too long"
            );

        public static readonly Error TooShort =
            new Error(
                "Name.TooShort",
                "Name is too short"
            );
    }
    public static class Airport
    {
        public static readonly Error CodeAlreadyInUse = new Error("Airport.Code"
            , "The code is already in use ");
        
        public static readonly Error NotFound = new Error("Airport.NotFound", "Airport is Not Found");

    }
    public static class Airline
    {
        public static readonly Error IATACodeAlreadyInUse =
            new Error("Airline.IATACode", "The IATACode is already in use ");
        
        public static readonly Error NotFound = new Error("Airline.NotFound", "Airline is Not Found");

    }
    
    public static class Flight
    {
        public static readonly Error FlightNumverAlreadyInUse =
            new Error("Flight.FlightNumber", "The FlightNumber is already in use");
        public static readonly Error OriginAirportNotFound =
            new Error("Flight.OriginAirportId", "The OriginAirport is Not Found");

        public static readonly Error DestinationAirportNotFound =
            new Error("Flight.DestinationAirportId", "The DestinationAirport is Not Found");

        public static readonly Error AirlineNotFound = 
            new Error("Flight.AirlineId", "The Airline is Not Found");

    }
    
    public static class Booking
    {
        public static readonly Error UserNotFound = 
            new Error("Flight.UserId", "The User is Not Found");

        public static readonly Error FlightNotFound =
            new Error("Flight.FlightId", "The Flight is Not Found") ;
    }
    
    public static class Seat
    {
        public static readonly Error SeatNumberAlreadyInUse = 
            new Error("Seat.SeatNumber", "The SeatNumber is Already in use");

        public static readonly Error FlightNotFound =
            new Error("Seat.FlightId", "The Flight is Not Found") ;

        public static readonly Error NotIsAvailable = 
            new Error("Seat.IsAvailable", "The Seats is Not IsAvailable") ;

    }
    
}