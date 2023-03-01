namespace IDOS.Models;

using System.Text.Json.Serialization;

public sealed class StopGroup {
	public Stop[] Stops { get; init; }
	public bool IsTrain { get; init; }
	[JsonPropertyName("uniqueName")]
	public string Name { get; init; }
	[JsonPropertyName("avgLat")]
	public float AverageLatitude { get; init; }
	[JsonPropertyName("avgLon")]
	public float AverageLongitude { get; init; }
	public string Municipality { get; init; }
}