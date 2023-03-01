namespace IDOS.Models;

using System.Text.Json.Serialization;

public sealed class Stop {
	public Line[] Lines { get; init; }
	/// <summary>
	/// The internal identifier of the stop node number / column number.
	/// This identifier is unique within the entire file and immutable between file updates.
	/// The node number corresponds to the node value of the stop group.
	/// </summary>
	public string Id { get; init; }
	[JsonPropertyName("altIdosName")]
	public string Name { get; init; }
	[JsonPropertyName("lat")]
	public float Latitude { get; init; }
	[JsonPropertyName("lon")]
	public float Longitude { get; init; }
}