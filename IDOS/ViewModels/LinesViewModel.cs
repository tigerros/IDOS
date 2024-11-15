namespace IDOS.ViewModels;

using System.ComponentModel.DataAnnotations;
using Models;

public sealed class LinesViewModel : QueryPagesViewModel<Line> {
	[MaxLength(100, ErrorMessage = "Too long! The maximum length is 100.")]
	[Display(Name = "Line name")]
	public override string? Query { get; init; }

	[Display(Name = "Type filter")] public List<CheckBox> TrafficTypes { get; init; } = _trafficTypes.ToList();

	private static readonly CheckBox[] _trafficTypes;
	
	static LinesViewModel() {
		string[] trafficTypes = Enum.GetNames<TrafficType>();
	
		_trafficTypes = new CheckBox[trafficTypes.Length];
	
		for (int i = 0; i < trafficTypes.Length; i++) {
			_trafficTypes[i] = new CheckBox(trafficTypes[i], true);
		}
	}
}