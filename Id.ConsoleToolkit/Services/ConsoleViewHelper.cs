namespace Id.ConsoleToolkit.Services
{
	/// <summary>
	/// Helper for console view.
	/// </summary>
	/// <remarks>
	/// Constructor for <see cref="ConsoleViewHelper"/>.
	/// </remarks>
	/// <param name="localizer">Localizer</param>
	public class ConsoleViewHelper(IStringLocalizer<ConsoleViewHelper> localizer) : IConsoleViewHelper
	{
		private readonly IStringLocalizer<ConsoleViewHelper> _t = localizer;

		/// <summary>
		/// Shows a message in the console.
		/// </summary>
		/// <param name="titleKey">Title key for selection</param>
		/// <param name="options">array of options</param>
		/// <returns>string Selection</returns>
		public string Select(string titleKey, params (string Value, string TextKey)[] options)
		{
			List<LocalizedMenuItem> choices = [.. options.Select(option => new LocalizedMenuItem
			{
				Value = option.Value,
				DisplayText = _t[option.TextKey]
			})];

			LocalizedMenuItem selected = AnsiConsole.Prompt(
				new SelectionPrompt<LocalizedMenuItem>()
					.Title(_t[titleKey])
					.AddChoices(choices));

			return selected.Value;
		}

		/// <summary>
		/// Shows the title in the console.
		/// </summary>
		/// <param name="text">Text to be shown as the title</param>
		/// <param name="color">Spectre console color from enum Color</param>
		public void ShowTitle(string text, Color? color = null)
		{
			FigletText figlet = new FigletText(text)
				.Centered();

			if(color.HasValue)
			{
				_ = figlet.Color(color.Value);
			}
			else
			{
				_ = figlet.Color(Color.Yellow2);
			}

			AnsiConsole.Write(figlet);
			AnsiConsole.WriteLine();
		}

		/// <summary>
		/// Shows a success message in the console.
		/// </summary>
		/// <param name="message">The message text</param>
		public void ShowSuccess(string message)
		{
			AnsiConsole.MarkupLine($"[bold green]✓ {message}[/]");
		}

		/// <summary>
		/// Shows an error message in the console.
		/// </summary>
		/// <param name="message">The message text</param>
		public void ShowError(string message)
		{
			AnsiConsole.MarkupLine($"[bold red]✗ {message}[/]");
		}

		private class LocalizedMenuItem
		{
			public string Value { get; set; } = string.Empty;
			public string DisplayText { get; set; } = string.Empty;

			public override string ToString() => DisplayText;
		}
	}
}