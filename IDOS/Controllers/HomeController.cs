using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IDOS.ViewModels;

namespace IDOS.Controllers;

using Models;
using Services;

public class HomeController : Controller {
	private readonly StopGroupService _stopGroupService;
	private readonly LineService _lineService;
	
	public HomeController(StopGroupService stopGroupService, LineService lineService) {
		_stopGroupService = stopGroupService;
		_lineService = lineService;
	}
	
	public IActionResult Index() => View(new BaseViewModel());

	public IActionResult StopGroups() =>
		View(new StopGroupsViewModel {
			QueryResults = 
				_stopGroupService.StopGroups[StopGroupsViewModel.DefaultRangeStart..StopGroupsViewModel.DefaultRangeEnd],
		});

	[HttpGet]
	public IActionResult StopGroups(StopGroupsViewModel formResponse) {
		return View(new StopGroupsViewModel {
			Query = formResponse.Query,
			Page = formResponse.Page,
			ItemsPerPage = formResponse.ItemsPerPage,
			QueryResults = StopGroupsViewModel.GetQueryResults(formResponse.Query,
				formResponse.Page,
				formResponse.ItemsPerPage,
				emptyQueryResult: _stopGroupService.StopGroups,
				validQueryResult: query => _stopGroupService.Search(query)),
		});
	}

	public IActionResult Lines() =>
		View(new LinesViewModel {
			QueryResults = _lineService.Lines[LinesViewModel.DefaultRangeStart..LinesViewModel.DefaultRangeEnd],
		});

	[HttpGet]
	public IActionResult Lines(LinesViewModel formResponse) {
		Console.WriteLine("Traffic types length: " + formResponse.TrafficTypes.Count);
		foreach (CheckBox checkBox in formResponse.TrafficTypes) {
			Console.WriteLine($"{checkBox.Text} checked: {checkBox.Checked}");
		}
		return View(new LinesViewModel {
			Query = formResponse.Query,
			Page = formResponse.Page,
			ItemsPerPage = formResponse.ItemsPerPage,
			QueryResults = LinesViewModel.GetQueryResults(formResponse.Query, 
				formResponse.Page,	
				formResponse.ItemsPerPage,
				emptyQueryResult: _lineService.Lines,
				validQueryResult: query => _lineService.Search(query)),
			TrafficTypes = formResponse.TrafficTypes,
		});
	}

	public IActionResult StopGroupDetails(string stopGroupName) {
		StopGroup? group = _stopGroupService.GetGroup(stopGroupName);
		
		return group == null ?
			View("Error", new ErrorViewModel { Message = $"Unable to find a stop group with the name \"{stopGroupName}\"", }) :
			View(new StopGroupDetailsViewModel { StopGroup = group, StopGroupLines = _lineService.GetStopGroupLines(group), });
	}
	
	public IActionResult LineDetails(string lineName) {
		Line? line = _lineService.GetLine(lineName);
		
		return line == null ?
			View("Error", new ErrorViewModel { Message = $"Unable to find a line with the name \"{lineName}\"", }) :
			View(new LineDetailsViewModel { Line = line, LineStopGroups = _lineService.GetLineStopGroups(line.Id), });
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error(string message) =>
		View(new ErrorViewModel {
			RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
			Message = message,
		});
}