using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FontBuddyLib
{
	/// <summary>
	/// Provides static utility methods for text layout operations such as word wrapping, scaling, and position calculations.
	/// </summary>
	public static class LineFormatter
	{
		/// <summary>
		/// Breaks a text string into multiple lines that fit within the specified width.
		/// Words are kept whole and sentences are given two spaces after sentence-ending punctuation.
		/// </summary>
		/// <param name="text">The text to break into lines.</param>
		/// <param name="rowWidth">The maximum width in pixels for each line.</param>
		/// <param name="font">The font renderer to use for measuring text.</param>
		/// <returns>A list of strings, each representing a line of text. Returns a single-item list with the original text if it fits.</returns>
		public static List<string> BreakTextIntoList(string text, int rowWidth, IFontBuddy font)
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

		/// <summary>
		/// Calculates the scale factor needed to make the text exactly fit within the specified width.
		/// </summary>
		/// <param name="text">The text to scale.</param>
		/// <param name="rowWidth">The target width in pixels.</param>
		/// <param name="font">The font renderer to use for measuring text.</param>
		/// <returns>The scale multiplier to apply to make the text fit the width exactly.</returns>
		public static float ScaleToFit(string text, int rowWidth, IFontBuddy font)
		{
			//measure the text
			var textSize = font.MeasureString(text);

			//get the ratio to scale the width to fit
			return rowWidth / textSize.X;
		}

		/// <summary>
		/// Calculates the scale factor needed to shrink the text to fit within the specified width.
		/// Returns 1.0 if the text already fits.
		/// </summary>
		/// <param name="text">The text to potentially shrink.</param>
		/// <param name="rowWidth">The maximum width in pixels.</param>
		/// <param name="font">The font renderer to use for measuring text.</param>
		/// <returns>The scale multiplier (1.0 or less) to make the text fit.</returns>
		public static float ShrinkToFit(string text, int rowWidth, IFontBuddy font)
		{
			//measure the text
			var textSize = font.MeasureString(text);

			//get the ratio to scale the width to fit
			return textSize.X > rowWidth ? rowWidth / textSize.X : 1f;
		}

		/// <summary>
		/// Determines whether the text needs to be shrunk to fit within the specified width at the given scale.
		/// </summary>
		/// <param name="text">The text to check.</param>
		/// <param name="scale">The current scale multiplier.</param>
		/// <param name="rowWidth">The maximum width in pixels.</param>
		/// <param name="font">The font renderer to use for measuring text.</param>
		/// <returns><c>true</c> if the text exceeds the row width at the given scale; otherwise, <c>false</c>.</returns>
		public static bool NeedsToShrink(string text, float scale, int rowWidth, IFontBuddy font)
		{
			//measure the text
			var textSize = font.MeasureString(text) * scale;

			//get the ratio to scale the width to fit
			return textSize.X > rowWidth;
		}

		/// <summary>
		/// Calculates the adjusted position for text based on the justification mode.
		/// </summary>
		/// <param name="text">The text to position.</param>
		/// <param name="position">The base position.</param>
		/// <param name="font">The font renderer to use for measuring text.</param>
		/// <param name="justification">The text justification mode. Default is <see cref="Justify.Left"/>.</param>
		/// <param name="scale">The scale multiplier for the font size. Default is 1.0.</param>
		/// <returns>The adjusted position based on justification. For Left, returns the original position. For Right, shifts left by text width. For Center, shifts left by half the text width.</returns>
		public static Vector2 JustifiedPosition(string text, Vector2 position, IFontBuddy font, Justify justification = Justify.Left, float scale = 1f)
		{
			//Get the correct location
			var textSize = (!string.IsNullOrEmpty(text) ? (font.MeasureString(text) * scale) : Vector2.Zero);

			switch (justification)
			{
				//left = use teh x value (no cahnge)

				case Justify.Right:
					{
						//move teh x value
						position.X -= textSize.X;
					}
					break;

				case Justify.Center:
					{
						//move teh x value
						position.X -= (textSize.X / 2.0f);
					}
					break;
			}

			return position;
		}

		/// <summary>
		/// Calculates the adjusted position for text rotated 90 degrees clockwise based on the justification mode.
		/// Adjusts the Y position instead of X since the text is rotated.
		/// </summary>
		/// <param name="text">The text to position.</param>
		/// <param name="position">The base position.</param>
		/// <param name="font">The font renderer to use for measuring text.</param>
		/// <param name="justification">The text justification mode. Default is <see cref="Justify.Left"/>.</param>
		/// <param name="scale">The scale multiplier for the font size. Default is 1.0.</param>
		/// <returns>The adjusted position for rotated text based on justification.</returns>
		public static Vector2 GetRotate90JustifiedPosition(string text, Vector2 position, IFontBuddy font, Justify justification = Justify.Left, float scale = 1f)
		{
			//Get the correct location
			var textSize = (!string.IsNullOrEmpty(text) ? (font.MeasureString(text) * scale) : Vector2.Zero);

			switch (justification)
			{
				//left = use teh x value (no cahnge)

				case Justify.Right:
					{
						//move teh x value
						position.Y -= textSize.X;
					}
					break;

				case Justify.Center:
					{
						//move teh x value
						position.Y -= (textSize.X / 2.0f);
					}
					break;
			}

			return position;
		}

		/// <summary>
		/// Calculates the adjusted position for text rotated 270 degrees clockwise (90 degrees counter-clockwise) based on the justification mode.
		/// Adjusts the Y position instead of X since the text is rotated.
		/// </summary>
		/// <param name="text">The text to position.</param>
		/// <param name="position">The base position.</param>
		/// <param name="font">The font renderer to use for measuring text.</param>
		/// <param name="justification">The text justification mode. Default is <see cref="Justify.Left"/>.</param>
		/// <param name="scale">The scale multiplier for the font size. Default is 1.0.</param>
		/// <returns>The adjusted position for rotated text based on justification.</returns>
		public static Vector2 GetRotate270JustifiedPosition(string text, Vector2 position, IFontBuddy font, Justify justification = Justify.Left, float scale = 1f)
		{
			//Get the correct location
			var textSize = (!string.IsNullOrEmpty(text) ? (font.MeasureString(text) * scale) : Vector2.Zero);

			switch (justification)
			{
				//left = use teh x value (no cahnge)

				case Justify.Right:
					{
						//move teh x value
						position.Y += textSize.X;
					}
					break;

				case Justify.Center:
					{
						//move teh x value
						position.Y += (textSize.X / 2.0f);
					}
					break;
			}

			return position;
		}
	}
}
