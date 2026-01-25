using System;
using System.Collections.Generic;

namespace FontBuddyLib
{
	/// <summary>
	/// Internal cache for splitting text strings into individual character strings.
	/// Used by effects that need to process text character by character (e.g., rainbow, shaky).
	/// </summary>
	internal class FontStringCache
	{
		/// <summary>
		/// Gets the cached list of single-character strings.
		/// </summary>
		public List<String> StringCache { get; private set; }

		/// <summary>
		/// Gets or sets the original text that was cached.
		/// </summary>
		private string Text { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="FontStringCache"/> class.
		/// </summary>
		public FontStringCache()
		{
			StringCache = new List<string>();
		}

		/// <summary>
		/// Updates the cache with the specified text, splitting it into individual character strings.
		/// Only rebuilds the cache if the text has changed.
		/// </summary>
		/// <param name="text">The text to cache.</param>
		public void UpdateText(string text)
		{
			if (text != Text)
			{
				Text = text;
				StringCache.Clear();
				for (int i = 0; i < Text.Length; i++)
				{
					//get teh size of the character
					StringCache.Add(text[i].ToString());
				}
			}
		}
	}
}
