using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FontBuddyLib
{
	/// <summary>
	/// Specifies the horizontal text alignment mode.
	/// </summary>
	public enum Justify
	{
		/// <summary>
		/// Left-aligned text. The position specifies the upper-left corner.
		/// </summary>
		Left,

		/// <summary>
		/// Right-aligned text. The position specifies the upper-right corner.
		/// </summary>
		Right,

		/// <summary>
		/// Center-aligned text. The position specifies the upper-center point.
		/// </summary>
		Center
	}

	/// <summary>
	/// A simple wrapper class for drawing text using MonoGame/XNA SpriteFonts.
	/// Provides convenient methods for measuring, positioning, and rendering text with various justification options.
	/// </summary>
	public class FontBuddy : IFontBuddy
	{
		#region Properties

		/// <summary>
		/// Gets or sets the underlying SpriteFont used for rendering text.
		/// </summary>
		protected virtual SpriteFont Font { get; set; }

		/// <summary>
		/// Gets or sets the sprite effects to apply when rendering text (e.g., horizontal or vertical flip).
		/// </summary>
		public virtual SpriteEffects SpriteEffects { get; set; }

		/// <summary>
		/// Gets or sets the rotation angle in radians to apply when rendering text.
		/// </summary>
		public virtual float Rotation { get; set; }

		/// <summary>
		/// Gets the character spacing of the font.
		/// </summary>
		public float Spacing => Font.Spacing;

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="FontBuddy"/> class.
		/// You must call <see cref="LoadContent"/> or set the <see cref="Font"/> property before use.
		/// </summary>
		public FontBuddy()
		{
			SpriteEffects = SpriteEffects.None;
		}

		/// <summary>
		/// Releases resources used by this instance. This implementation does nothing as SpriteFonts are managed by the ContentManager.
		/// </summary>
		public void Dispose()
		{
		}

		/// <summary>
		/// Measures the dimensions of the specified text string.
		/// </summary>
		/// <param name="text">The text string to measure.</param>
		/// <returns>A <see cref="Vector2"/> containing the width (X) and height (Y) of the text in pixels.</returns>
		public Vector2 MeasureString(string text)
		{
			return Font.MeasureString(text);
        }

		/// <summary>
		/// Loads a SpriteFont resource from the content manager.
		/// </summary>
		/// <param name="content">The content manager to load the font from.</param>
		/// <param name="resource">The name of the font resource to load.</param>
		/// <param name="useFontBuddyPlus">Must be <c>false</c> for this class. Throws an exception if <c>true</c>.</param>
		/// <param name="fontSize">Not used by this class.</param>
		/// <exception cref="Exception">Thrown when <paramref name="useFontBuddyPlus"/> is <c>true</c>.</exception>
		public void LoadContent(ContentManager content, string resource, bool useFontBuddyPlus = false, int fontSize = 0)
		{
			if (useFontBuddyPlus)
			{
				throw new Exception("FontBuddy.LoadContent was passed useFontBuddyPlus = true");
			}

			//load font
			if (null == Font)
			{
				Font = content.Load<SpriteFont>(resource);
			}
		}

		/// <summary>
		/// Renders text to the screen with the specified parameters.
		/// </summary>
		/// <param name="text">The text to render.</param>
		/// <param name="position">The position to render at. Interpretation depends on justification.</param>
		/// <param name="justification">The text justification mode.</param>
		/// <param name="scale">The scale multiplier for the font size.</param>
		/// <param name="color">The color to render the text in.</param>
		/// <param name="spriteBatch">The SpriteBatch to use for rendering.</param>
		/// <param name="time">The game clock. Not used by this class but provided for interface compatibility.</param>
		/// <returns>The X position at the end of the rendered text.</returns>
		public virtual float Write(string text,
			Vector2 position,
			Justify justification,
			float scale,
			Color color,
			SpriteBatch spriteBatch,
			GameClock time)
		{
			return DrawText(text, position, justification, scale, color, spriteBatch, time);
		}

		/// <summary>
		/// Internal method that performs the actual text rendering with justification.
		/// </summary>
		/// <param name="text">The text to render.</param>
		/// <param name="position">The position to render at. Interpretation depends on justification.</param>
		/// <param name="justification">The text justification mode.</param>
		/// <param name="scale">The scale multiplier for the font size.</param>
		/// <param name="color">The color to render the text in.</param>
		/// <param name="spriteBatch">The SpriteBatch to use for rendering.</param>
		/// <param name="time">The game clock for time-based effects.</param>
		/// <returns>The X position at the end of the rendered text, or the original X position if text is empty.</returns>
		protected float DrawText(string text,
			Vector2 position,
			Justify justification,
			float scale,
			Color color,
			SpriteBatch spriteBatch,
			GameClock time)
		{
			//if this thing is empty, dont do anything
			if (string.IsNullOrEmpty(text))
			{
				return position.X;
			}

			position = LineFormatter.JustifiedPosition(text, position, this, justification, scale);

			//okay, draw the actual string
			DrawString(text, position, scale, color, spriteBatch);

			//return the end of that string
			return position.X + (MeasureString(text).X * scale);
		}

		/// <summary>
		/// Draws a text string at the specified position without justification calculations.
		/// </summary>
		/// <param name="text">The text to draw.</param>
		/// <param name="position">The upper-left position to draw at.</param>
		/// <param name="scale">The scale multiplier for the font size.</param>
		/// <param name="color">The color to draw the text in.</param>
		/// <param name="spriteBatch">The SpriteBatch to use for rendering.</param>
		public void DrawString(string text, Vector2 position, float scale, Color color, SpriteBatch spriteBatch)
		{
			spriteBatch.DrawString(Font,
									text,
									position,
									color,
									Rotation,
									Vector2.Zero,
									scale,
									SpriteEffects,
									0);
		}

		/// <summary>
		/// Breaks a text string into multiple lines that fit within the specified width.
		/// </summary>
		/// <param name="text">The text to break into lines.</param>
		/// <param name="rowWidth">The maximum width in pixels for each line.</param>
		/// <returns>A list of strings, each representing a line of text.</returns>
		public List<string> BreakTextIntoList(string text, int rowWidth)
		{
			return LineFormatter.BreakTextIntoList(text, rowWidth, this);
		}

		/// <summary>
		/// Calculates the scale factor needed to make the text exactly fit within the specified width.
		/// </summary>
		/// <param name="text">The text to scale.</param>
		/// <param name="rowWidth">The target width in pixels.</param>
		/// <returns>The scale multiplier to apply to make the text fit the width exactly.</returns>
		public float ScaleToFit(string text, int rowWidth)
		{
			return LineFormatter.ScaleToFit(text, rowWidth, this);
		}

		/// <summary>
		/// Calculates the scale factor needed to shrink the text to fit within the specified width.
		/// Returns 1.0 if the text already fits.
		/// </summary>
		/// <param name="text">The text to potentially shrink.</param>
		/// <param name="rowWidth">The maximum width in pixels.</param>
		/// <returns>The scale multiplier (1.0 or less) to make the text fit.</returns>
		public float ShrinkToFit(string text, int rowWidth)
		{
			return LineFormatter.ShrinkToFit(text, rowWidth, this);
		}

		/// <summary>
		/// Determines whether the text needs to be shrunk to fit within the specified width at the given scale.
		/// </summary>
		/// <param name="text">The text to check.</param>
		/// <param name="scale">The current scale multiplier.</param>
		/// <param name="rowWidth">The maximum width in pixels.</param>
		/// <returns><c>true</c> if the text exceeds the row width at the given scale; otherwise, <c>false</c>.</returns>
		public bool NeedsToShrink(string text, float scale, int rowWidth)
		{
			return LineFormatter.NeedsToShrink(text, scale, rowWidth, this);
		}

		#endregion //Methods
	}
}