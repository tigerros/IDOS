namespace IDOS.ViewModels;

public class ErrorViewModel : BaseViewModel {
	public string RequestId { get; set; } = "Unknown";
	public string Message { get; set; } = "None";

	public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}