namespace Id.ConsoleToolkit.Helpers
{
	/// <summary>
	/// Interface for a browser helper.
	/// </summary>
	public interface IBrowser
	{
		/// <summary>
		/// Gets or sets the actual folder.
		/// </summary>
		string ActualFolder { get; set; }

		/// <summary>
		/// Gets or sets the actual folder text.
		/// </summary>
		string ActualFolderText { get; set; }

		/// <summary>
		/// Gets or sets the cancel text.
		/// </summary>
		bool CanCreateFolder { get; set; }

		/// <summary>
		/// Gets or sets the cancel text.
		/// </summary>
		string CreateNewText { get; set; }

		/// <summary>
		/// Gets or sets the cancel text.
		/// </summary>
		bool DisplayIcons { get; set; }

		/// <summary>
		/// Gets or sets the list of drives.
		/// </summary>
		string[] Drives { get; set; }

		/// <summary>
		/// Gets or sets the boolean saying if we work with Windows based OS.
		/// </summary>
		bool IsWindows { get; }

		/// <summary>
		/// Last folder used.
		/// </summary>
		string LastFolder { get; set; }

		/// <summary>
		/// Gets or sets the level up text.
		/// </summary>
		string LevelUpText { get; set; }

		/// <summary>
		/// Gets or sets the more choices text.
		/// </summary>
		string MoreChoicesText { get; set; }

		/// <summary>
		/// Defines the amount of items to be displayed on the screen.
		/// </summary>
		int PageSize { get; set; }

		/// <summary>
		/// Gets or sets the text for "Select actual folder".
		/// </summary>
		string SelectActualText { get; set; }

		/// <summary>
		/// Gets or sets the text for "Select drive".
		/// </summary>
		string SelectDriveText { get; set; }

		/// <summary>
		/// Gets or sets the selected file path.
		/// </summary>
		string SelectedFile { get; set; }

		/// <summary>
		/// Gets or sets the text for "Select file".
		/// </summary>
		string SelectFileText { get; set; }

		/// <summary>
		/// Gets or sets the text for "Select folder".
		/// </summary>
		string SelectFolderText { get; set; }

		/// <summary>
		/// Method to get the file path.
		/// </summary>
		/// <returns>The file path</returns>
		string GetFilePath();

		/// <summary>
		/// Method to get the file path from the actual folder.
		/// </summary>
		/// <param name="ActualFolder">Path to the actual folder</param>
		/// <returns></returns>
		string GetFilePath(string ActualFolder);

		/// <summary>
		/// Method to get the path for the actual folder.
		/// </summary>
		/// <returns></returns>
		string GetFolderPath();

		/// <summary>
		/// Method to get the path for the actual folder, from the actual folder.
		/// </summary>
		/// <param name="ActualFolder"></param>
		/// <returns></returns>
		string GetFolderPath(string ActualFolder);

		/// <summary>
		/// Method to get the path for the actual file, from the actual folder.
		/// </summary>
		/// <param name="ActualFolder">The path to the actual folder</param>
		/// <param name="SelectFile">Defines if we want the path to the folder or to the certain file</param>
		/// <returns></returns>
		string GetPath(string ActualFolder, bool SelectFile);
	}
}