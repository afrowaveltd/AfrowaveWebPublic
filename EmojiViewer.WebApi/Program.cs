WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
WebApplication app = builder.Build();

app.UseDefaultFiles(); // Loads index.html
app.UseStaticFiles();  // Enables serving from wwwroot

app.MapGet("/api/emojis", () =>
{
	string path = Path.Combine("Jsons", "EmojiData.json");
	if(!File.Exists(path))
	{
		return Results.NotFound("EmojiData.json not found.");
	}

	string json = File.ReadAllText(path);
	return Results.Content(json, "application/json");
});

app.Run();