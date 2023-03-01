namespace IDOS.ViewModels;

using Models;

public sealed class StopGroupDetailsViewModel : BaseViewModel {
	public StopGroup StopGroup { get; init; }
	public Line[] StopGroupLines { get; init; } = Array.Empty<Line>();
}