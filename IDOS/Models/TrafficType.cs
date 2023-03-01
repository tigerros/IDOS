namespace IDOS.Models;

using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TrafficType {
	Metro,
	Tram,
	Train,
	Funicular,
	Bus,
	Ferry,
	Trolleybus,
}