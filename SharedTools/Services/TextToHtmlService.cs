namespace SharedTools.Services
{
	using System;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Web;

	/// <summary>
	/// TextToHtmlService is a class that is used to convert text to HTML.
	/// </summary>
	public class TextToHtmlService : ITextToHtmlService
	{
		/// <summary>
		/// ConvertTextToHtml is a method that takes a string input and returns a string output.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public string ConvertTextToHtml(string input)
		{
			if(string.IsNullOrWhiteSpace(input))
			{
				return ""; // Return an empty string if the input is null or whitespace
			}

			StringBuilder htmlBuilder = new StringBuilder();
			string[] lines = input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

			bool inParagraph = false;
			bool inList = false; // Track if we are inside an unordered list

			foreach(string line in lines)
			{
				if(string.IsNullOrWhiteSpace(line)) // Empty line signals the end of a paragraph or list
				{
					if(inParagraph)
					{
						_ = htmlBuilder.Append("</p>\n");
						inParagraph = false;
					}
					if(inList)
					{
						_ = htmlBuilder.Append("</ul>\n");
						inList = false;
					}
					continue;
				}

				string trimmedLine = line.TrimStart();
				int leadingSpaces = line.Length - trimmedLine.Length;

				if(trimmedLine.StartsWith("-")) // Handle unordered list items
				{
					if(!inList)
					{
						if(inParagraph)
						{
							_ = htmlBuilder.Append("</p>\n");
							inParagraph = false;
						}
						_ = htmlBuilder.Append("<ul>\n");
						inList = true;
					}
					string listItem = trimmedLine.Substring(1).Trim(); // Remove the "-" symbol and trim whitespace
					_ = htmlBuilder.AppendFormat("<li>{0}</li>\n", HttpUtility.HtmlEncode(listItem));
				}
				else if(line.StartsWith("\t")) // Handle lines with tabs (e.g., links)
				{
					if(inList)
					{
						_ = htmlBuilder.Append("</ul>\n");
						inList = false;
					}
					_ = htmlBuilder.Append(ProcessLinkLine(trimmedLine) + "\n");
				}
				else if(leadingSpaces >= 2) // Determine header level based on leading spaces
				{
					if(inList)
					{
						_ = htmlBuilder.Append("</ul>\n");
						inList = false;
					}
					int headerLevel = Math.Min(leadingSpaces / 2, 6); // Map spaces to header levels (max <h6>)
					_ = htmlBuilder.AppendFormat("<h{0}>{1}</h{0}>\n", headerLevel, HttpUtility.HtmlEncode(trimmedLine));
				}
				else
				{
					if(inList)
					{
						_ = htmlBuilder.Append("</ul>\n");
						inList = false;
					}
					if(!inParagraph)
					{
						_ = htmlBuilder.Append("<p>");
						inParagraph = true;
					}

					string processedLine = ProcessLineForLinks(trimmedLine);
					_ = htmlBuilder.Append(processedLine + " ");
				}
			}

			// Close any open paragraph or list at the end
			if(inParagraph)
			{
				_ = htmlBuilder.Append("</p>\n");
			}
			if(inList)
			{
				_ = htmlBuilder.Append("</ul>\n");
			}

			return htmlBuilder.ToString();
		}

		private string ProcessLineForLinks(string line)
		{
			// Regular expressions for detecting HTTP/HTTPS links and email addresses
			string linkPattern = @"\b(https?:\/\/[^\s]+)\b"; // Matches HTTP/HTTPS links
			string emailPattern = @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}\b"; // Matches email addresses

			// Step 1: Replace links with <a> tags
			line = Regex.Replace(line, linkPattern, match =>
			{
				string url = match.Value;
				return $"<a href=\"{HttpUtility.HtmlEncode(url)}\" target=\"_blank\" rel=\"noopener noreferrer\">{HttpUtility.HtmlEncode(url)}</a>";
			}, RegexOptions.IgnoreCase);

			// Step 2: Replace email addresses with <a> mailto: links
			line = Regex.Replace(line, emailPattern, match =>
			{
				string email = match.Value;
				return $"<a href=\"mailto:{HttpUtility.HtmlEncode(email)}\">{HttpUtility.HtmlEncode(email)}</a>";
			});

			// Return the transformed line (non-link portions are already encoded by the input processing logic)
			return line;
		}

		private string ProcessLinkLine(string line)
		{
			string[] parts = line.Split(new[] { '\t' }, 2);
			if(parts.Length < 2)
			{
				return HttpUtility.HtmlEncode(line); // No tab found, return as-is
			}

			string textBeforeTab = HttpUtility.HtmlEncode(parts[0]);
			string linkPart = parts[1];

			int urlEndIndex = linkPart.IndexOf('['); // Look for custom link body
			if(urlEndIndex >= 0)
			{
				string url = linkPart.Substring(0, urlEndIndex).Trim();
				int linkBodyEnd = linkPart.IndexOf(']', urlEndIndex);
				if(linkBodyEnd > urlEndIndex)
				{
					string linkBody = linkPart.Substring(urlEndIndex + 1, linkBodyEnd - urlEndIndex - 1).Trim();
					string afterLink = linkPart.Substring(linkBodyEnd + 1).Trim();

					return $"{textBeforeTab}<a href=\"{HttpUtility.HtmlEncode(url)}\" target=\"_blank\">{HttpUtility.HtmlEncode(linkBody)}</a> {HttpUtility.HtmlEncode(afterLink)}";
				}
			}

			// Default case: no custom link body
			string urlOnly = linkPart.Trim();
			return $"{textBeforeTab}<a href=\"{HttpUtility.HtmlEncode(urlOnly)}\" target=\"_blank\">{HttpUtility.HtmlEncode(urlOnly)}</a>";
		}
	}
}