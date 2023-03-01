namespace IDOS.ViewModels;

using Models;

public sealed class LineDetailsViewModel : BaseViewModel {
	public Line Line { get; init; }
	public StopGroup[] LineStopGroups { get; init; } = Array.Empty<StopGroup>();
}