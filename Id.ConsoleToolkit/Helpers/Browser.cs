using Spectre.Console;

namespace Id.ConsoleToolkit.Helpers
{
	/// <summary>
	/// The Spectre.Console File Browser is a simple file browser that allows you to select files and folders.
	/// </summary>
	public class Browser : IBrowser
	{
		private readonly IStringLocalizer<Browser> _t;

		/// <summary>
		/// DisplayIcons is a boolean property that determines whether to display icons in the file browser.
		/// </summary>
		public bool DisplayIcons { get; set; } = true;

		/// <summary>
		/// IsWindows is a boolean property that indicates whether the operating system is Windows.
		/// </summary>
		public bool IsWindows { get; } = true;

		/// <summary>
		/// PageSize is an integer property that determines the number of items displayed per page in the file browser.
		/// </summary>
		public int PageSize { get; set; } = 15;

		/// <summary>
		/// CanCreateFolder is a boolean property that indicates whether the user can create a new folder in the file browser.
		/// </summary>
		public bool CanCreateFolder { get; set; } = true;

		/// <summary>
		/// ActualFolder is a string property that represents the current folder being browsed.
		/// </summary>
		public string ActualFolder { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

		/// <summary>
		/// SelectedFile is a string property that represents the file selected by the user.
		/// </summary>
		public string SelectedFile { get; set; } = string.Empty;

		/// <summary>
		/// SelectedFolder is a string property that represents the folder selected by the user.
		/// </summary>
		public string LevelUpText { get; set; } = "Go to upper level";

		/// <summary>
		/// ActualFolderText is a string property that represents the text displayed for the current folder.
		/// </summary>
		public string ActualFolderText { get; set; } = "Selected Folder";

		/// <summary>
		/// MoreChoicesText is a string property that represents the text displayed for more choices in the file browser.
		/// </summary>
		public string MoreChoicesText { get; set; } = "Use arrows Up and Down to select";

		/// <summary>
		/// CreateNewText is a string property that represents the text displayed for creating a new folder.
		/// </summary>
		public string CreateNewText { get; set; } = "Create new folder";

		/// <summary>
		/// SelectFileText is a string property that represents the text displayed for selecting a file.
		/// </summary>
		public string SelectFileText { get; set; } = "Select File";

		/// <summary>
		/// SelectFolderText is a string property that represents the text displayed for selecting a folder.
		/// </summary>
		public string SelectFolderText { get; set; } = "Select Folder";

		/// <summary>
		/// SelectDriveText is a string property that represents the text displayed for selecting a drive.
		/// </summary>
		public string SelectDriveText { get; set; } = "Select Drive";

		/// <summary>
		/// SelectActualText is a string property that represents the text displayed for selecting the actual folder.
		/// </summary>
		public string SelectActualText { get; set; } = "Select Actual Folder";

		/// <summary>
		/// Drives is a string array property that contains the list of drives available on the system.
		/// </summary>
		public string[] Drives { get; set; } = [];

		/// <summary>
		/// lastFolder is a string property that represents the last folder accessed in the file browser.
		/// </summary>
		public string LastFolder { get; set; } = string.Empty;

		/// <summary>
		/// The constructor initializes the Browser class and sets the IsWindows property based on the current operating system.
		/// </summary>

		public Browser(IStringLocalizer<Browser> t)
		{
			string OS = Environment.OSVersion.Platform.ToString();
			if(OS[..3].Equals("win", StringComparison.CurrentCultureIgnoreCase))
			{
				IsWindows = true;
			}
			_t = t;

			LastFolder = ActualFolder;
		}

		/// <summary>
		/// GetPath is a method that displays the file browser and allows the user to select a file or folder.
		/// </summary>
		/// <param name="ActualFolder">The actual folder</param>
		/// <param name="SelectFile">The selected file</param>
		/// <returns>Full path to the selected file</returns>
		public string GetPath(string ActualFolder, bool SelectFile)
		{
			string lastFolder = ActualFolder;
			while(true)
			{
				string headerText = SelectFile ? SelectFileText : SelectFolderText;
				string[] directoriesInFolder;
				Directory.SetCurrentDirectory(ActualFolder ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));

				AnsiConsole.Clear();
				AnsiConsole.WriteLine();
				Rule rule = new Rule($"[b][green]{headerText}[/][/]").Centered();
				AnsiConsole.Write(rule);

				AnsiConsole.WriteLine();
				AnsiConsole.Markup($"[b][Yellow]{ActualFolderText}: [/][/]");
				TextPath path = new(ActualFolder?.ToString() ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))
				{
					RootStyle = new Style(foreground: Color.Green),
					SeparatorStyle = new Style(foreground: Color.Green),
					StemStyle = new Style(foreground: Color.Blue),
					LeafStyle = new Style(foreground: Color.Yellow)
				};
				AnsiConsole.Write(path);
				AnsiConsole.WriteLine();

				Dictionary<string, string> folders = [];
				// get list of drives

				try
				{
					directoriesInFolder = Directory.GetDirectories(Directory.GetCurrentDirectory());
					lastFolder = ActualFolder ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
				}
				catch
				{
					if(ActualFolder == lastFolder)
					{
						ActualFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
					}
					else
					{
						ActualFolder = lastFolder ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
					}

					Directory.SetCurrentDirectory(ActualFolder);
					directoriesInFolder = Directory.GetDirectories(Directory.GetCurrentDirectory());
				}

				if(IsWindows)
				{
					if(DisplayIcons)
					{
						folders.Add("[green]:computer_disk: " + SelectDriveText + "[/]", "/////");
					}
					else
					{
						folders.Add("[green]" + SelectDriveText + "[/]", "/////");
					}
				}
				try
				{
					if(new DirectoryInfo(ActualFolder ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)).Parent != null)
					{
						if(DisplayIcons)
						{
							folders.Add("[green]:upwards_button: " + LevelUpText + "[/]", new DirectoryInfo(ActualFolder ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)).Parent?.FullName ?? null!);
						}
						else
						{
							folders.Add("[green]" + LevelUpText + "[/]", new DirectoryInfo(ActualFolder ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)).Parent?.FullName ?? null!);
						}
					}
				}
				catch { }
				if(!SelectFile)
				{
					if(DisplayIcons)
					{
						folders.Add(":ok_button: [green]" + SelectActualText + "[/]", Directory.GetCurrentDirectory());
					}
					else
					{
						folders.Add("[green]" + SelectActualText + "[/]", Directory.GetCurrentDirectory());
					}
				}
				if(CanCreateFolder)
				{
					if(DisplayIcons)
					{
						folders.Add("[green]:plus: " + CreateNewText + "[/]", "///new");
					}
					else
					{
						folders.Add("[green]" + CreateNewText + "[/]", "///new");
					}
				}
				foreach(string d in directoriesInFolder)
				{
					int cut = 0;
					if(new DirectoryInfo(ActualFolder ?? null!).Parent != null)
					{
						cut = 1;
					}

					string FolderName = d[((ActualFolder?.Length ?? 0) + cut)..];
					string FolderPath = d;
					if(DisplayIcons)
					{
						folders.Add(":file_folder: " + FolderName, FolderPath);
					}
					else
					{
						folders.Add(FolderName, FolderPath);
					}
				}

				if(SelectFile)
				{
					string[] fileList = Directory.GetFiles(ActualFolder ?? null!);
					foreach(string file in fileList)
					{
						string result = Path.GetFileName(file);
						if(DisplayIcons)
						{
							folders.Add(":abacus: " + result, file);
						}
						else
						{
							folders.Add(result, file);
						}
					}
				}
				// We got two sets of lists list files and list folders
				string title = SelectFile ? SelectFileText : SelectFolderText;
				string selected = AnsiConsole.Prompt(
					 new SelectionPrompt<string>()
						  .Title($"[green]{title}:[/]")
						  .PageSize(PageSize)
						  .MoreChoicesText($"[grey]{MoreChoicesText}[/]")
						  .AddChoices(folders.Keys)
					 );
				lastFolder = ActualFolder ?? null!;
				string? record = folders.Where(s => s.Key == selected).Select(s => s.Value).FirstOrDefault();
				if(record == "/////")
				{
					record = SelectDrive();
					ActualFolder = record;
				}
				if(record == "///new")
				{
					string folderName = AnsiConsole.Ask<string>("[blue]" + CreateNewText + ": [/]");
					if(folderName != null)
					{
						try
						{
							_ = Directory.CreateDirectory(folderName);
							string newFolder = Path.Combine(ActualFolder ?? null!, folderName);
							record = newFolder;
						}
						catch(Exception ex)
						{
							AnsiConsole.WriteLine("[red]Error: [/]" + ex.Message);
						}
					}
				}
				string responseType;
				if(Directory.Exists(record))
				{
					responseType = "Directory";
				}
				else
				{
					responseType = "File";
				}

				if(record == Directory.GetCurrentDirectory())
				{
					return ActualFolder ?? null!;
				}

				if(responseType == "Directory")
				{
					try
					{
						ActualFolder = record ?? null!;
					}
					catch
					{
						AnsiConsole.WriteLine("[red]You have no access to this folder[/]");
					}
				}
				else
				{
					return record ?? null!;
				}
			}
		}

		/// <summary>
		/// GetFilePath is a method that displays the file browser and allows the user to select a file.
		/// </summary>
		/// <param name="ActualFolder">Actual folder path</param>
		/// <returns>File path</returns>
		public string GetFilePath(string ActualFolder)
		{
			return GetPath(ActualFolder, true);
		}

		/// <summary>
		/// GetFilePath is a method that displays the file browser and allows the user to select a file.
		/// </summary>
		/// <returns>Path string</returns>
		public string GetFilePath()
		{
			return GetPath(ActualFolder, true);
		}

		/// <summary>
		/// GetFolderPath is a method that displays the file browser and allows the user to select a folder.
		/// </summary>
		/// <param name="ActualFolder"></param>
		/// <returns>Path string</returns>
		public string GetFolderPath(string ActualFolder)
		{
			return GetPath(ActualFolder, false);
		}

		/// <summary>
		/// GetFolderPath is a method that displays the file browser and allows the user to select a folder.
		/// </summary>
		/// <returns>Path string</returns>
		public string GetFolderPath()
		{
			return GetPath(ActualFolder, false);
		}

		private string SelectDrive()
		{
			Drives = Directory.GetLogicalDrives();
			Dictionary<string, string> result = [];
			foreach(string drive in Drives)
			{
				if(DisplayIcons)
				{
					result.Add(":computer_disk: " + drive, drive);
				}
				else
				{
					result.Add(drive, drive);
				}
			}
			AnsiConsole.Clear();
			AnsiConsole.WriteLine();
			Rule rule = new Rule($"[b][green]{SelectDriveText}[/][/]").Centered();
			AnsiConsole.Write(rule);

			AnsiConsole.WriteLine();
			string title = SelectDriveText;
			string selected = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					 .Title($"[green]{title}:[/]")
					 .PageSize(PageSize)
					 .MoreChoicesText($"[grey]{MoreChoicesText}[/]")
					 .AddChoices(result.Keys)
				);
			string? record = result.Where(s => s.Key == selected).Select(s => s.Value).FirstOrDefault();
			return record ?? string.Empty;
		}
	}
}