namespace Id.ConsoleToolkit.Views
{
	/// <summary>
	/// Main menu view.
	/// </summary>
	/// <param name="t">Localizer</param>
	/// <param name="viewHelper">Helper for the layout of the view</param>
	public class MainMenuView(
	 IConsoleViewHelper viewHelper,
	 IStringLocalizer<MainMenuView> t) : ConsoleViewBase(viewHelper), IMainMenuView
	{
		private readonly IStringLocalizer<MainMenuView> _t = t;
		private readonly IConsoleViewHelper _viewHelper = viewHelper;

		/// <summary>
		/// Non mandatory Title at the top
		/// </summary>
		protected override string? Title => _t["Main Menu"];

		/// <summary>
		/// Each view must implement its own logic for rendering and interaction.
		/// </summary>
		protected override Task ShowAsync()
		{
			_ = Console.ReadKey();
			return Task.CompletedTask;
		}
	}
}