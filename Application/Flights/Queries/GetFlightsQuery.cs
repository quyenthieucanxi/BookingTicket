using Application.Abstractions.Messaging;
using Domain.Shared;

namespace Application.Flights.Queries;

public record GetFlightsQuery(string? SearchTerm, string? SortColumn, string? SortOrder,int Page,int PageSize) 
    : IQuery<PageList<FlightResponse>>;