using Id.ConsoleToolkit.Views.Base;

namespace Id.ConsoleToolkit.Views
{
	public class MainMenuView(IStringLocalizer<MainMenuView> t) : ConsoleViewBase
	{
		private readonly IStringLocalizer<MainMenuView> _t = t;

		/// <summary>
		/// Non mandatory Title at the top
		/// </summary>
		protected override string? Title => _t["Main Menu"];

		/// <summary>
		/// Each view must implement its own logic for rendering and interaction.
		/// </summary>
		protected override Task ShowAsync()
		{
			Console.WriteLine("Main Menu");
			return Task.CompletedTask;
		}
	}
}