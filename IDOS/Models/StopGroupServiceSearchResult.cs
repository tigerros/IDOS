namespace IDOS.Models; 

// Goofy ahh jméno
public readonly record struct StopGroupServiceSearchResult(StopGroup StopGroup, int LowestWordDistanceFromQuery, int TotalDistanceFromQuery);