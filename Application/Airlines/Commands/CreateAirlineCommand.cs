using Application.Abstractions.Messaging;

namespace Application.Airlines.Commands;

public sealed record CreateAirlineCommand(string Name,string Country,string IATACode) : ICommand;