namespace IDOS.TagHelpers ;

using System.Text;
using Controllers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Models;
using Services;

[HtmlTargetElement("stop-group-table", TagStructure = TagStructure.NormalOrSelfClosing)]
public class StopGroupTableTagHelper : TagHelper {
	public ModelExpression StopGroups { get; set; }

	public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
		if (StopGroups.ModelExplorer.ModelType != typeof(StopGroup[])) {
			throw new Exception("The provided attribute is not a HashSet<StopGroup>.");
		}

		output.TagName = "table";

		var content = new StringBuilder(@"<thead>
<tr>
	<th>Name</th>
	<th>Latitude</th>
	<th>Longitude</th>
	<th>Municipality</th>
</tr>
</thead>
<tbody>");
		
		// Goofy ahh anchor (manual asp-controller asp-action asp-route)
		// Not worth creating a whole "utility" class for one method to generate it. Hate them "utility" classes
		foreach (StopGroup stopGroup in (StopGroup[]) StopGroups.Model) {
			content.Append($@"<tr>
<td>{stopGroup.Name}</td>
<td>{stopGroup.AverageLatitude.ToString("00.000000")}</td>
<td>{stopGroup.AverageLongitude.ToString("00.000000")}</td>
<td>{stopGroup.Municipality}</td>
<td>
<a href=""/{nameof(HomeController).Replace("Controller", "")}/{nameof(HomeController.StopGroupDetails)}?stopGroupName={stopGroup.Name}"">Details</a></td>
</tr>");
		}
	
		output.Content.SetHtmlContent(content.ToString());

		return base.ProcessAsync(context, output);
	}
}