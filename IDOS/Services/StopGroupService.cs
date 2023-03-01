namespace IDOS.Services;

using Helpers;
using Models;

public sealed class StopGroupService {
	public StopGroup[] StopGroups { get; }

	public StopGroupService(StopGroup[] stopGroups) {
		StopGroups = stopGroups;
	}
	
	public StopGroup? GetGroup(string groupName) {
		groupName = groupName.RemoveDiacritics();

		foreach (StopGroup stopGroup in StopGroups) {
			if (stopGroup.Name.RemoveDiacritics() == groupName) return stopGroup;
		}

		return null;
	}

	public StopGroup[] Search(string groupNameQuery) {
		groupNameQuery = groupNameQuery.RemoveDiacritics();

		var searchResults = new List<StopGroupServiceSearchResult>();
		string[] queryWords = groupNameQuery.Split(' ');
		
		foreach (StopGroup stopGroup in StopGroups) {
			string name = stopGroup.Name.RemoveDiacritics();
			
			if (name == groupNameQuery) {
				searchResults.Add(new StopGroupServiceSearchResult(stopGroup, 0, 0));
				continue;
			}

			int lowestWordDistance = LowestWordDistance(name, queryWords);

			if (lowestWordDistance > (name.Length >> 2)) continue;

			int totalDistance = StringHelper.LevenshteinDistance(name, groupNameQuery);
			
			searchResults.Add(new StopGroupServiceSearchResult(stopGroup, lowestWordDistance, totalDistance));
		}

		// Maybe optimize this LINQ query later	
		return searchResults.OrderBy(sr => sr.TotalDistanceFromQuery).ThenBy(sr => sr.LowestWordDistanceFromQuery).Select(sr => sr.StopGroup).ToArray();
	}

	private static int LowestWordDistance(string stopGroupName, string[] queryWords) {
		int lowestWordDistance = int.MaxValue;
		
		foreach (string word in stopGroupName.Split(',')) {
			foreach (string queryWord in queryWords) {
				int wordDistance = StringHelper.LevenshteinDistance(word, queryWord);

				if (wordDistance < lowestWordDistance) lowestWordDistance = wordDistance;
				if (wordDistance == 0) break;
			}

			if (lowestWordDistance == 0) break;
		}

		return lowestWordDistance;
	}

	private static double DegreeesToRadians(double degrees) => degrees * 0.017453292519943295; // degrees * (PI / 180)

	private static int SecondsBetweenPoints(float lat1, float lon1, float lat2, float lon2, TrafficType trafficType) {
		int kmh = trafficType switch {
			TrafficType.Metro => 36,
			TrafficType.Tram => 20,
			TrafficType.Train => 70, // It's complicated, so I'm going with a guess. Hopefully accurate for the commuter trains, obviously different for intercity trains
			TrafficType.Funicular => 6,
			TrafficType.Bus => 25,
			TrafficType.Ferry => 5, // Estimating. Not enough data because ferries don't operate in the winter.
			TrafficType.Trolleybus => 25, // Couldn't find exact info, so I'm going with the speed the bus has
			_ => throw new ArgumentOutOfRangeException(nameof(trafficType), trafficType, null),
		};
		
		// <https://stackoverflow.com/a/27943/15683397>
		const int radius = 6371; // Radius of the earth in km
		
		double dLat = DegreeesToRadians(lat2 - lat1);
		double dLon = DegreeesToRadians(lon2 - lon1);
		
		double a = (Math.Sin(dLat / 2) * Math.Sin(dLat / 2)) + 
		           (Math.Cos(DegreeesToRadians(lat1)) * Math.Cos(DegreeesToRadians(lat2)) * 
		            Math.Sin(dLon / 2) * Math.Sin(dLon / 2));
		
		double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a)); 
		double d = radius * c; // Distance in km
		
		return (int)Math.Round((d / kmh) * 3600); // time = (distance / speed) // * 3600 converts hours into seconds
	}

	// private List<StopGroup> _fastestPath;
	//
	// public StopGroup[] FastestPathBetween(StopGroup stopGroup1, StopGroup stopGroup2, LineService lineService) {
	// 	Line[] lines1 = lineService.GetStopGroupLines(stopGroup1);
	// 	Line[] lines2 = lineService.GetStopGroupLines(stopGroup2);
	// 	_fastestPath = new List<StopGroup>();
	// 	
	// 	foreach (Line line1 in lines1) {
	// 		StopGroup[] line1StopGroups = lineService.GetLineStopGroups(line1.Id);
	//
	// 		foreach (Line line2 in lines2) {
	// 			StopGroup[] line2StopGroups = lineService.GetLineStopGroups(line2.Id);
	//
	// 			foreach (StopGroup line1StopGroup in line1StopGroups) {
	// 				foreach (StopGroup line2StopGroup in line2StopGroups) {
	// 					if (line1StopGroup.Name == line2StopGroup.Name) {
	// 						return _fastestPath.ToArray();
	// 					}
	// 				}
	// 			}
	// 		}
	// 	}
	//
	// 	return FastestPathBetween();
	// }
}