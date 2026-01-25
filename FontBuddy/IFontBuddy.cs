using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FontBuddyLib
{
	/// <summary>
	/// Defines the contract for font rendering implementations in FontBuddy.
	/// Provides methods for loading fonts, measuring text, and rendering text with various justification options.
	/// </summary>
	public interface IFontBuddy : IDisposable
	{
		/// <summary>
		/// Gets or sets the sprite effects to apply when rendering text (e.g., horizontal or vertical flip).
		/// </summary>
		SpriteEffects SpriteEffects { get; set; }

		/// <summary>
		/// Gets or sets the rotation angle in radians to apply when rendering text.
		/// </summary>
		float Rotation { get; set; }

		/// <summary>
		/// Loads a font resource from the content manager.
		/// </summary>
		/// <param name="content">The content manager to load the font from.</param>
		/// <param name="resourceName">The name of the font resource to load.</param>
		/// <param name="useFontBuddyPlus">If <c>true</c>, uses FontStashSharp for advanced font rendering; otherwise uses standard SpriteFont.</param>
		/// <param name="fontSize">The font size in points when using FontBuddyPlus. Ignored for standard SpriteFont.</param>
		void LoadContent(ContentManager content, string resourceName, bool useFontBuddyPlus = false, int fontSize = 24);

		/// <summary>
		/// Measures the dimensions of the specified text string.
		/// </summary>
		/// <param name="text">The text string to measure.</param>
		/// <returns>A <see cref="Vector2"/> containing the width (X) and height (Y) of the text in pixels.</returns>
		Vector2 MeasureString(string text);

		/// <summary>
		/// Gets the character spacing of the font.
		/// </summary>
		float Spacing { get; }

		/// <summary>
		/// Renders text to the screen with the specified parameters.
		/// </summary>
		/// <param name="text">The text to render.</param>
		/// <param name="position">The position to render at. Interpretation depends on justification: upper-left for Left, upper-center for Center, upper-right for Right.</param>
		/// <param name="justification">The text justification mode.</param>
		/// <param name="scale">The scale multiplier for the font size. This is not a point size; it's a multiplier of the default font size.</param>
		/// <param name="color">The color to render the text in.</param>
		/// <param name="spriteBatch">The SpriteBatch to use for rendering.</param>
		/// <param name="time">The game clock for time-based effects. Can be null or zero for static text.</param>
		/// <returns>The X position at the end of the rendered text.</returns>
		float Write(string text,
			Vector2 position,
			Justify justification,
			float scale,
			Color color,
			SpriteBatch spriteBatch,
			GameClock time);

		/// <summary>
		/// Draws a text string at the specified position without justification calculations.
		/// </summary>
		/// <param name="text">The text to draw.</param>
		/// <param name="position">The upper-left position to draw at.</param>
		/// <param name="scale">The scale multiplier for the font size.</param>
		/// <param name="color">The color to draw the text in.</param>
		/// <param name="spriteBatch">The SpriteBatch to use for rendering.</param>
		void DrawString(string text, Vector2 position, float scale, Color color, SpriteBatch spriteBatch);

		/// <summary>
		/// Breaks a text string into multiple lines that fit within the specified width.
		/// </summary>
		/// <param name="text">The text to break into lines.</param>
		/// <param name="rowWidth">The maximum width in pixels for each line.</param>
		/// <returns>A list of strings, each representing a line of text.</returns>
		List<string> BreakTextIntoList(string text, int rowWidth);

		/// <summary>
		/// Calculates the scale factor needed to make the text exactly fit within the specified width.
		/// </summary>
		/// <param name="text">The text to scale.</param>
		/// <param name="rowWidth">The target width in pixels.</param>
		/// <returns>The scale multiplier to apply to make the text fit the width exactly.</returns>
		float ScaleToFit(string text, int rowWidth);

		/// <summary>
		/// Calculates the scale factor needed to shrink the text to fit within the specified width.
		/// Returns 1.0 if the text already fits.
		/// </summary>
		/// <param name="text">The text to potentially shrink.</param>
		/// <param name="rowWidth">The maximum width in pixels.</param>
		/// <returns>The scale multiplier (1.0 or less) to make the text fit.</returns>
		float ShrinkToFit(string text, int rowWidth);

		/// <summary>
		/// Determines whether the text needs to be shrunk to fit within the specified width at the given scale.
		/// </summary>
		/// <param name="text">The text to check.</param>
		/// <param name="scale">The current scale multiplier.</param>
		/// <param name="rowWidth">The maximum width in pixels.</param>
		/// <returns><c>true</c> if the text exceeds the row width at the given scale; otherwise, <c>false</c>.</returns>
		bool NeedsToShrink(string text, float scale, int rowWidth);
	}
}