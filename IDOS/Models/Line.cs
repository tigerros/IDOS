namespace IDOS.Models;

using System.Text.Json.Serialization;

public sealed class Line {
	/// <summary>
	/// Numerical identificator of the line.
	/// </summary>
	public int Id { get; init; }
	/// <summary>
	/// The name of the line passed on to the passengers.
	/// </summary>
	public string Name { get; init; }
	/// <summary>
	/// The traffic (vehicle) type.
	/// </summary>
	public TrafficType Type { get; init; }
	/// <summary>
	/// <c>true</c> if the line only operates at night; otherwise, <c>false</c>.
	/// </summary>
	public bool IsNight { get; init; }
	/// <summary>
	/// The most common terminus for the line departing from the given stop.
	/// </summary>
	public string Direction { get; init;  }
	/// <summary>
	/// Alternative terminus if the line has alternating termini.
	/// Optional.
	/// </summary>
	[JsonPropertyName("direction2")]
	public string? AlternativeDirection { get; init; }

	public HashSet<string> Directions { get; } = new();
}