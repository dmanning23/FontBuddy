using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FontBuddyLib
{
	internal static class LineFormatter
	{
		public static List<string> BreakTextIntoList(string text, int rowWidth, SpriteFont font)
		{
			//Check for invalid parameters or text that fits on a single line
			if ((rowWidth <= 0) || string.IsNullOrEmpty(text) || (font.MeasureString(text).X <= rowWidth))
			{
				return new List<string> { text };
			}

			//Break the text up into words
			string[] words = text.Split(' ');

			//Add words to the list until they go over the length
			List<string> lines = new List<string>();
			int currentWord = 0;
			while (currentWord < words.Length)
			{
				int wordsThisLine = 0;
				string line = string.Empty;
				while (currentWord < words.Length)
				{
					string testLine = line;
					if (testLine.Length < 1)
					{
						testLine += words[currentWord];
					}
					else if ((testLine[testLine.Length - 1] == '.') ||
						(testLine[testLine.Length - 1] == '?') ||
						(testLine[testLine.Length - 1] == '!'))
					{
						//Add two spaces at the end of a sentence
						testLine += "  " + words[currentWord];
					}
					else
					{
						//Add a space in between words
						testLine += " " + words[currentWord];
					}

					if ((wordsThisLine > 0) && (font.MeasureString(testLine).X > rowWidth))
					{
						//The latest word put it over the line width, but make sure not to add empty lines
						break;
					}

					line = testLine;
					wordsThisLine++;
					currentWord++;
				}
				lines.Add(line);
			}
			return lines;
		}

		public static float ScaleToFit(string text, int rowWidth, SpriteFont font)
		{
			//measure the text
			var textSize = font.MeasureString(text);

			//get the ratio to scale the width to fit
			return rowWidth / textSize.X;
		}

		public static float ShrinkToFit(string text, int rowWidth, SpriteFont font)
		{
			//measure the text
			var textSize = font.MeasureString(text);

			//get the ratio to scale the width to fit
			return textSize.X > rowWidth ? rowWidth / textSize.X : 1f;
		}

		public static bool NeedsToShrink(string text, float scale, int rowWidth, SpriteFont font)
		{
			//measure the text
			var textSize = font.MeasureString(text) * scale;

			//get the ratio to scale the width to fit
			return textSize.X > rowWidth;
		}
	}
}
