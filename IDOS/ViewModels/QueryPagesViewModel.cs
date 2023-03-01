namespace IDOS.ViewModels;

using System.ComponentModel.DataAnnotations;

public abstract class QueryPagesViewModel<TQueryResult> : BaseViewModel {
	private const int MinimumPage = 0;
	private const int MinimumItemsPerPage = 5;
	private const int DefaultPage = 0; // Must be >= MinimumPage
	private const int DefaultItemsPerPage = 20; // Must be >= MinimumItemsPerPage
	public const int DefaultRangeStart = DefaultPage * DefaultItemsPerPage;
	public const int DefaultRangeEnd = (DefaultPage + 1) * DefaultItemsPerPage;
	
	/// <summary>
	/// The query property is abstract so that you need to add a display name and validation to it.
	/// </summary>
	[DataType(DataType.Text)]
	public abstract string? Query { get; init; }

	/// <summary>
	/// The page to display.
	/// </summary>
	[DataType(DataType.Currency)]
	[Display(Name = "Page number")]
	[Range(MinimumPage, int.MaxValue, ErrorMessage = "Only positive numbers allowed.")]
	public int Page { get; init; } = DefaultPage;

	/// <summary>
	/// The amount of items shown per page.
	/// </summary>
	[DataType(DataType.Currency)]
	[Display(Name = "Items per page")]
	[Range(MinimumItemsPerPage, int.MaxValue)]
	public int ItemsPerPage { get; init; } = DefaultItemsPerPage;

	/// <summary>
	/// The results generated with the query.
	/// Only a segment will be displayed, where the start is <c>Page * ItemsPerPage</c> and the end
	/// is <c>(Page + 1) * ItemsPerPage</c>.
	/// </summary>
	public TQueryResult[] QueryResults { get; init; } = Array.Empty<TQueryResult>();

	private static (int rangeStart, int rangeEnd) GetRangeBounds(int page, int itemsPerPage, int resultsLength) {
		int rangeStart = page * itemsPerPage;
		int rangeEnd = (page + 1) * itemsPerPage;

		rangeStart = Math.Clamp(rangeStart, 0, resultsLength - (itemsPerPage + 1));
		rangeEnd = Math.Clamp(rangeEnd, itemsPerPage - 1, resultsLength - 1);

		return (rangeStart, rangeEnd);
	}

	/// <summary>
	/// Returns an array that represents the segment of the query results,
	/// defined by the <paramref name="page"/> and <paramref name="itemsPerPage"/>.
	/// </summary>
	/// <param name="query">The query string.</param>
	/// <param name="page">The page number. Will be clamped according to the <see cref="MinimumPage"/>.</param>
	/// <param name="itemsPerPage">
	/// The amount of items shown per page.
	/// Will be clamped according to the <see cref="MinimumItemsPerPage"/>.
	/// If the query result length will be less than <see cref="MinimumItemsPerPage"/>, that length will be used instead.
	/// </param>
	/// <param name="emptyQueryResult">The query results if the <paramref name="query"/> is empty or null.</param>
	/// <param name="validQueryResult">The query result function if the <paramref name="query"/> is not empty or null.</param>
	public static TQueryResult[] GetQueryResults(string? query, int page, int itemsPerPage, TQueryResult[] emptyQueryResult, Func<string, TQueryResult[]> validQueryResult) {
		TQueryResult[] results = string.IsNullOrEmpty(query) ? emptyQueryResult : validQueryResult(query);

		if (results.Length == 0) {
			return results;
		}

		page = Math.Clamp(page, MinimumPage, int.MaxValue);
		itemsPerPage = Math.Min(results.Length, itemsPerPage);
		
		(int rangeStart, int rangeEnd) = GetRangeBounds(page, itemsPerPage, results.Length);

		return results[rangeStart..rangeEnd];
	}
}