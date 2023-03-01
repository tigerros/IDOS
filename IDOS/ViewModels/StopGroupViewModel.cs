namespace IDOS.ViewModels;

using System.ComponentModel.DataAnnotations;
using Models;

public sealed class StopGroupsViewModel : QueryPagesViewModel<StopGroup> {
	[MinLength(2, ErrorMessage = "Too short! The minimum length is 2.")]
	[MaxLength(100, ErrorMessage = "Too long! The maximum length is 100.")]
	[Display(Name = "Stop group name")]
	public override string? Query { get; init; }
}