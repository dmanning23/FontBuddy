using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FontBuddyLib
{
	/// <summary>
	/// Abstract base class for font effect implementations.
	/// Provides common functionality for text rendering with effects by wrapping an underlying <see cref="IFontBuddy"/> instance.
	/// </summary>
	public abstract class BaseFontBuddy : IFontBuddy
	{
		#region Properties

		/// <summary>
		/// Gets or sets the underlying font renderer used for drawing text.
		/// </summary>
		public IFontBuddy Font { get; set; }

		/// <summary>
		/// Gets the character spacing of the underlying font.
		/// </summary>
		public float Spacing => Font.Spacing;

		private SpriteEffects _spriteEffects;

		/// <summary>
		/// Gets or sets the sprite effects to apply when rendering text (e.g., horizontal or vertical flip).
		/// Setting this value also updates the underlying font's sprite effects.
		/// </summary>
		public SpriteEffects SpriteEffects
		{
			get
			{
				return _spriteEffects;
			}
			set
			{
				_spriteEffects = value;
				if (null != Font)
				{
					Font.SpriteEffects = SpriteEffects;
				}
			}
		}

		private float _rotation;

		/// <summary>
		/// Gets or sets the rotation angle in radians to apply when rendering text.
		/// Setting this value also updates the underlying font's rotation.
		/// </summary>
		public float Rotation
		{
			get
			{
				return _rotation;
			}
			set
			{
				_rotation = value;
				if (null != Font)
				{
					Font.Rotation = Rotation;
				}
			}
		}

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Releases resources used by this instance.
		/// </summary>
		public abstract void Dispose();

		/// <summary>
		/// Loads a font resource from the content manager and creates the underlying font renderer.
		/// </summary>
		/// <param name="content">The content manager to load the font from.</param>
		/// <param name="resourceName">The name of the font resource to load.</param>
		/// <param name="useFontBuddyPlus">If <c>true</c>, uses FontStashSharp for advanced font rendering; otherwise uses standard SpriteFont.</param>
		/// <param name="fontSize">The font size in points when using FontBuddyPlus. Ignored for standard SpriteFont.</param>
		public void LoadContent(ContentManager content, string resourceName, bool useFontBuddyPlus = false, int fontSize = 24)
		{
			if (useFontBuddyPlus)
			{
				//throw new NotImplementedException("FontBuddyPlus is not working right now.");
				Font = new FontBuddyPlus();
			}
			else
			{
				Font = new FontBuddy();
				Font.SpriteEffects = SpriteEffects;
			}
			Font.Rotation = Rotation;

			Font.LoadContent(content, resourceName, useFontBuddyPlus, fontSize);
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
		/// Breaks a text string into multiple lines that fit within the specified width.
		/// </summary>
		/// <param name="text">The text to break into lines.</param>
		/// <param name="rowWidth">The maximum width in pixels for each line.</param>
		/// <returns>A list of strings, each representing a line of text.</returns>
		public List<string> BreakTextIntoList(string text, int rowWidth)
		{
			return Font.BreakTextIntoList(text, rowWidth);
		}

		/// <summary>
		/// Calculates the scale factor needed to make the text exactly fit within the specified width.
		/// </summary>
		/// <param name="text">The text to scale.</param>
		/// <param name="rowWidth">The target width in pixels.</param>
		/// <returns>The scale multiplier to apply to make the text fit the width exactly.</returns>
		public float ScaleToFit(string text, int rowWidth)
		{
			return Font.ScaleToFit(text, rowWidth);
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
			return Font.ShrinkToFit(text, rowWidth);
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
			return Font.NeedsToShrink(text, scale, rowWidth);
		}

		/// <summary>
		/// Renders text to the screen with the specified parameters and applies the effect.
		/// </summary>
		/// <param name="text">The text to render.</param>
		/// <param name="position">The position to render at. Interpretation depends on justification.</param>
		/// <param name="justification">The text justification mode.</param>
		/// <param name="scale">The scale multiplier for the font size.</param>
		/// <param name="color">The color to render the text in.</param>
		/// <param name="spriteBatch">The SpriteBatch to use for rendering.</param>
		/// <param name="time">The game clock for time-based effects.</param>
		/// <returns>The X position at the end of the rendered text.</returns>
		public abstract float Write(string text, Vector2 position, Justify justification, float scale, Color color, SpriteBatch spriteBatch, GameClock time);

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
			Font.DrawString(text, position, scale, color, spriteBatch);
		}

#endregion //Methods
	}
}
