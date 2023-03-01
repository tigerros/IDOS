namespace IDOS.Models; 

// Taky goofy jméno ale ne tak hrozný jak StopGroupServiceSearchResult
public readonly record struct LineServiceSearchResult(Line Line, int DistanceFromQuery);