using Spectre.Console;

namespace Id.ConsoleToolkit.Views.Base
{
	/// <summary>
	/// Base class for console views.
	/// </summary>
	public abstract class ConsoleViewBase
	{
		/// <summary>
		/// Volitelný nadpis zobrazený nahoře v rámci <see cref="RunAsync"/>.
		/// </summary>
		protected virtual string? Title => null;

		/// <summary>
		/// Runs the console, renders view <see cref="ShowAsync"/>.
		/// </summary>
		public async Task RunAsync()
		{
			Console.Clear();

			if(!string.IsNullOrWhiteSpace(Title))
			{
				AnsiConsole.Write(new Rule($"[bold]{Title}[/]").RuleStyle("grey"));
			}

			await ShowAsync();
		}

		/// <summary>
		/// Each view must implement its own logic for rendering and interaction.
		/// </summary>
		protected abstract Task ShowAsync();
	}
}