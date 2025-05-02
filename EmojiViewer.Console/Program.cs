using EmojiViewer.Console.Models;
using Spectre.Console;
using System.Text.Json;
using TextCopy;

AnsiConsole.Write(new FigletText("Emoji Viewer").Centered().Color(Color.Yellow2));

string jsonPath = Path.Combine("Jsons", "EmojiData.json");
if(!File.Exists(jsonPath))
{
	AnsiConsole.MarkupLine("[red]EmojiData.json not found in Jsons/ directory.[/]");
	return;
}

string json = File.ReadAllText(jsonPath);
List<EmojiEntry>? emojis = JsonSerializer.Deserialize<List<EmojiEntry>>(json, new JsonSerializerOptions
{
	PropertyNameCaseInsensitive = true
});

if(emojis == null || emojis.Count == 0)
{
	AnsiConsole.MarkupLine("[red]Emoji list is empty or invalid.[/]");
	return;
}

while(true)
{
	EmojiEntry selected = AnsiConsole.Prompt(
		 new SelectionPrompt<EmojiEntry>()
			  .Title("[green]Select an emoji:[/]")
			  .PageSize(10)
			  .MoreChoicesText("(Use arrow keys and Enter to select)")
			  .UseConverter(e => $"{e.Utf8String}  {e.Name}")
			  .AddChoices(emojis)
	);

	string action = AnsiConsole.Prompt(
		 new SelectionPrompt<string>()
			  .Title("[blue]Choose an action:[/]")
			  .AddChoices("Show details", "Copy emoji", "Copy C#", "Exit")
	);

	switch(action)
	{
		case "Show details":
			AnsiConsole.WriteLine($"\nName: {selected.Name}");
			AnsiConsole.WriteLine($"Unicode: {selected.Utf8String}");
			AnsiConsole.WriteLine($"C#: {selected.CSharpString}");
			AnsiConsole.WriteLine($"Character: {selected.Utf8String}\n");
			break;

		case "Copy emoji":
			ClipboardService.SetText(selected.Utf8String ?? string.Empty);
			AnsiConsole.MarkupLine("[green]Emoji copied to clipboard![/]");
			break;

		case "Copy C#":
			ClipboardService.SetText(selected.CSharpString ?? string.Empty);
			AnsiConsole.MarkupLine("[green]C# string copied to clipboard![/]");
			break;

		case "Exit":
			return;
	}

	AnsiConsole.WriteLine();
}