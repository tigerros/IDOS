using System.Text.Json;
using IDOS.Models;
using IDOS.Services;

StopsJson? json = JsonSerializer.Deserialize<StopsJson>(
		File.ReadAllText("stops.json"),
		new JsonSerializerOptions(JsonSerializerDefaults.Web));

if (json == null) throw new JsonException("The JSON is not in the correct format.");

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(_ => new StopGroupService(json.StopGroups));
builder.Services.AddSingleton(provider => new LineService(provider.GetService<StopGroupService>() ?? throw new NullReferenceException("StopGroupService is null, but LineService needs it.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();