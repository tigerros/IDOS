namespace IDOS.Models; 

public sealed class StopsJson {
	public DateTime GeneratedAt { get; init; }
	public StopGroup[] StopGroups { get; init; }
}