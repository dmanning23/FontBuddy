using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FontBuddyLib
{
	/// <summary>
	/// A text renderer that draws each letter in a cycling rainbow of colors.
	/// The colors smoothly transition from one to the next, creating an animated rainbow effect.
	/// </summary>
	/// <remarks>
	/// Each letter is drawn individually with its color determined by the current time and position in the text.
	/// The color cycles through the <see cref="Colors"/> list at a rate controlled by <see cref="RainbowSpeed"/>.
	/// </remarks>
	public class RainbowTextBuddy : BaseFontBuddy, IShadowTextBuddy
	{
		#region Properties

		/// <summary>
		/// Gets or sets the color used to draw the text shadow.
		/// </summary>
		/// <value>The shadow color. Default is <see cref="Color.Black"/>.</value>
		public Color ShadowColor { get; set; }

		/// <summary>
		/// Gets or sets the pixel offset of the shadow from the main text position.
		/// </summary>
		/// <value>The shadow offset as a <see cref="Vector2"/>. Default is (0, 3).</value>
		public Vector2 ShadowOffset { get; set; }

		/// <summary>
		/// Gets or sets the scale multiplier for the shadow relative to the main text.
		/// </summary>
		/// <value>The shadow size multiplier. Default is 1.05.</value>
		public float ShadowSize { get; set; }

		/// <summary>
		/// Gets or sets the speed of the rainbow color cycling animation.
		/// Higher values result in faster color changes.
		/// </summary>
		/// <value>The rainbow animation speed. Default is 2.0.</value>
		public float RainbowSpeed { get; set; }

		/// <summary>
		/// Internal cache for individual character strings.
		/// </summary>
		private FontStringCache Text { get; set; }

		/// <summary>
		/// Gets or sets the list of colors to cycle through for the rainbow effect.
		/// </summary>
		/// <value>A list of colors. Default contains purple, blue, green, yellow, orange, and red.</value>
		public List<Color> Colors { get; set; }

		/// <summary>
		/// Gets or sets the kerning value used for character spacing.
		/// </summary>
		private float Kerning
		{
			get; set;
		}

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="RainbowTextBuddy"/> class with default rainbow colors.
		/// </summary>
		public RainbowTextBuddy()
		{
			ShadowColor = Color.Black;
			ShadowOffset = new Vector2(0.0f, 3.0f);
			ShadowSize = 1.05f;

			RainbowSpeed = 2.0f;
			Colors = new List<Color>();

			//well, this is the rainbow buddy, put some shit in there
			Colors.Add(Color.Purple);
			Colors.Add(Color.Blue);
			Colors.Add(Color.Green);
			Colors.Add(Color.Yellow);
			Colors.Add(Color.Orange);
			Colors.Add(Color.Red);

			Text = new FontStringCache();
		}

		/// <summary>
		/// Releases resources used by this instance.
		/// </summary>
		public override void Dispose()
		{
		}

		/// <summary>
		/// Renders text with a rainbow color cycling effect to the screen.
		/// Each letter is drawn in a different color that smoothly transitions over time.
		/// </summary>
		/// <param name="text">The text to render.</param>
		/// <param name="position">The position to render at. Interpretation depends on justification.</param>
		/// <param name="justification">The text justification mode.</param>
		/// <param name="scale">The scale multiplier for the font size.</param>
		/// <param name="color">The base color (not used directly; colors come from the Colors list).</param>
		/// <param name="spriteBatch">The SpriteBatch to use for rendering.</param>
		/// <param name="time">The game clock for the animation timing.</param>
		/// <returns>The X position at the end of the rendered text.</returns>
		public override float Write(string text,
			Vector2 position,
			Justify justification,
			float scale,
			Color color,
			SpriteBatch spriteBatch,
			GameClock time)
		{
			if (string.IsNullOrEmpty(text))
			{
				return position.X;
			}

			Kerning = Font.Spacing * scale;

			//update the string cache
			Text.UpdateText(text);

			//Get the correct location
			Vector2 textSize = MeasureString(text) * scale;
			switch (justification)
			{
				case Justify.Right:
				{
					//move teh x value
					for (var i = 0; i < Text.StringCache.Count; i++)
					{
						//get teh size of the character
						textSize = Font.MeasureString(Text.StringCache[i]) * scale;
						position.X -= textSize.X;

						//get the kerning too
						position.X -= Kerning;
					}
				}
				break;

				case Justify.Center:
				{
					//move teh x value
					for (var i = 0; i < Text.StringCache.Count; i++)
					{
						//get teh size of the character
						textSize = Font.MeasureString(Text.StringCache[i]) * scale;
						position.X -= (textSize.X / 2.0f);

						//get the kerning too
						position.X -= Kerning / 2.0f;
					}
				}
				break;
			}

			WriteShadow(text, position, justification, scale, spriteBatch, time);

			var cosRot = (float)System.Math.Cos(Rotation);
			var sinRot = (float)System.Math.Sin(Rotation);

			var currentTime = time.CurrentTime;
			currentTime *= RainbowSpeed;
			currentTime += text.Length;
			for (var i = 0; i < Text.StringCache.Count; i++)
			{
				//draw the title

				//Get the current color
				currentTime -= 0.6f;
				var index = (int)currentTime % Colors.Count;

				//get the next color
				var nextIndex = index + 1;
				if (nextIndex >= Colors.Count)
				{
					nextIndex = 0;
				}

				//get the ACTUAL lerped color of this letter
				var remainder = (float)(currentTime - (int)currentTime);
				var letterColor = Color.Lerp(Colors[index], Colors[nextIndex], remainder);

				var subString = Text.StringCache[i];

				Font.DrawString(subString, position, scale, letterColor, spriteBatch);

				var advance = (Font.MeasureString(subString) * scale).X + Kerning;
				position.X += advance * cosRot;
				position.Y += advance * sinRot;
			}

			//return the end of that string
			return position.X;
		}

		/// <summary>
		/// Draws the shadow portion of the text character by character.
		/// </summary>
		/// <param name="text">The text to draw as a shadow.</param>
		/// <param name="position">The base position for the text.</param>
		/// <param name="justification">The text justification mode.</param>
		/// <param name="scale">The scale multiplier for the font size.</param>
		/// <param name="spriteBatch">The SpriteBatch to use for rendering.</param>
		/// <param name="time">The game clock for time-based effects.</param>
		protected void WriteShadow(string text,
			Vector2 position,
			Justify justification,
			float scale,
			SpriteBatch spriteBatch,
			GameClock time)
		{
			var cosRot = (float)System.Math.Cos(Rotation);
			var sinRot = (float)System.Math.Sin(Rotation);

			for (int i = 0; i < Text.StringCache.Count; i++)
			{
				//draw the title
				var subString = Text.StringCache[i];

				Font.DrawString(subString, position + ShadowOffset, scale * ShadowSize, ShadowColor, spriteBatch);

				var advance = (Font.MeasureString(subString) * scale).X + Kerning;
				position.X += advance * cosRot;
				position.Y += advance * sinRot;
			}
		}

		#endregion //Methods
	}
}