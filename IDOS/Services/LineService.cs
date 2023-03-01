namespace IDOS.Services;

using Helpers;
using Models;

public sealed class LineService {
	private readonly StopGroupService _stopGroupService;
	public Line[] Lines { get; } = Array.Empty<Line>();

	public LineService(StopGroupService stopGroupService) {
		_stopGroupService = stopGroupService;
		
		var lineIdExistTable = new Dictionary<int, int>();
		var lines = new List<Line>();

		foreach (StopGroup stopGroup in _stopGroupService.StopGroups) {
			foreach (Stop stop in stopGroup.Stops) {
				foreach (Line line in stop.Lines) {
					if (lineIdExistTable.ContainsKey(line.Id)) {
						int index = lineIdExistTable[line.Id];

						lines[index].Directions.Add(line.Direction);

						if (line.AlternativeDirection != null) {
							lines[index].Directions.Add(line.AlternativeDirection);
						}

						continue;
					}
					
					lines.Add(line);
					lineIdExistTable.Add(line.Id, lines.Count - 1);
				}
			}
		}

		Lines = lines.ToArray();
	}
	
	public Line? GetLine(string name) {
		foreach (Line line in Lines) {
			if (line.Name == name) return line;
		}

		return null;
	}

	public StopGroup[] GetLineStopGroups(int id) {
		var stopGroups = new HashSet<StopGroup>();
		
		foreach (StopGroup stopGroup in _stopGroupService.StopGroups) {
			foreach (Stop stop in stopGroup.Stops) {
				foreach (Line line in stop.Lines) {
					if (line.Id == id) stopGroups.Add(stopGroup);
				}
			}
		}

		return stopGroups.ToArray();
	}

	public Line[] GetStopGroupLines(StopGroup stopGroup) {
		var stopGroupLines = new List<Line>();

		foreach (Stop stop in stopGroup.Stops) {
			foreach (Line line in stop.Lines) {
				stopGroupLines.Add(line);
			}
		}
		
		return stopGroupLines.ToArray();
	}

	public Line[] Search(string nameQuery) {
		nameQuery = nameQuery.Trim().RemoveDiacritics();

		var searchResults = new List<LineServiceSearchResult>();
		var searchResultsReturn = new List<Line>();

		foreach (Line line in Lines) {
			string lineName = line.Name.RemoveDiacritics();

			if (lineName == nameQuery) {
				searchResults.Insert(0, new LineServiceSearchResult(line, 0));
				searchResultsReturn.Insert(0, line);
				continue;
			}
			
			int distanceFromQuery = StringHelper.LevenshteinDistance(line.Name.RemoveDiacritics(), nameQuery);
			if (distanceFromQuery > 1) continue;

			var searchResult = new LineServiceSearchResult(line, distanceFromQuery);

			for (int sri = 0; sri < searchResults.Count; sri++) {
				LineServiceSearchResult otherSearchResult = searchResults[sri];

				if (sri == searchResults.Count - 1) {
					searchResults.Add(searchResult);
					searchResultsReturn.Add(line);
					break;
				}
				
				if (otherSearchResult.DistanceFromQuery > distanceFromQuery) {
					searchResults.Insert(sri, searchResult);
					searchResultsReturn.Insert(sri, line);
					break;
				}
			}
		}

		return searchResultsReturn.ToArray();
	}
}