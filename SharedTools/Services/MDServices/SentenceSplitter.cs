using System.Text.RegularExpressions;

namespace SharedTools.Services.MDServices
{
	/// <summary>
	/// Provides sentence splitting utilities for translation parsing.
	/// </summary>
	public static class SentenceSplitter
	{
		private static readonly Regex SentenceEndRegex = new(@"\s*(?<=[.!?])\s+", RegexOptions.Compiled);

		/// <summary>
		/// Splits a block of text into individual sentences using basic punctuation rules.
		/// </summary>
		/// <param name="text">Input text to split.</param>
		/// <returns>Array of sentence strings.</returns>
		public static string[] SplitIntoSentences(string text)
		{
			if(string.IsNullOrWhiteSpace(text))
			{
				return [];
			}

			return [.. SentenceEndRegex.Split(text).Where(s => !string.IsNullOrWhiteSpace(s))];
		}
	}
}