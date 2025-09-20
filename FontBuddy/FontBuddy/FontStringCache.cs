using System;
using System.Collections.Generic;

namespace FontBuddyLib
{
	internal class FontStringCache
	{
		public List<String> StringCache { get; private set; }
		private string Text { get; set; }

		public FontStringCache()
		{
			StringCache = new List<string>();
		}

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
