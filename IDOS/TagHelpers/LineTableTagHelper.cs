namespace IDOS.TagHelpers ;

using System.Text;
using Controllers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Models;

[HtmlTargetElement("line-table", TagStructure = TagStructure.NormalOrSelfClosing)]
public class LineTableTagHelper : TagHelper {
	public ModelExpression Lines { get; set; }
	
	public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
		if (Lines.ModelExplorer.ModelType != typeof(Line[])) {
			throw new Exception("The provided attribute is not a HashSet<StopGroup>.");
		}

		output.TagName = "table";

		var content = new StringBuilder(@"<thead>
<tr>
	<th>Name</th>
	<th>Type</th>
	<th>Is night?</th>
	<th>Directions</th>
</tr>
</thead>
<tbody>");
		
		foreach (Line line in (Line[]) Lines.Model) {
			content.Append($@"<tr>
<td>{line.Name}</td>
<td>{line.Type}</td>
<td>{line.IsNight}</td>
<td>{string.Join(" <--> ", line.Directions)}</td>
<td><a href=""/{nameof(HomeController).Replace("Controller", "")}/{nameof(HomeController.LineDetails)}?lineName={line.Name}"">Details</a></td>
</tr>");
		}
	
		output.Content.SetHtmlContent(content.ToString());

		return base.ProcessAsync(context, output);
	}
}