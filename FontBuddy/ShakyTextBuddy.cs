using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FontBuddyLib
{
	/// <summary>
	/// A text renderer that creates a shaking/trembling text effect.
	/// Each letter oscillates vertically in an alternating pattern.
	/// </summary>
	/// <remarks>
	/// The effect draws each letter individually with a vertical offset determined by a sine wave.
	/// Alternating letters move in opposite directions to create a vibrating appearance.
	/// </remarks>
	public class ShakyTextBuddy : BaseFontBuddy
	{
		#region Properties

		/// <summary>
		/// Gets or sets the maximum vertical displacement of the shake effect in pixels.
		/// Higher values result in more dramatic shaking.
		/// </summary>
		/// <value>The shake amplitude. Default is 10.</value>
		public float ShakeAmount { get; set; }

		/// <summary>
		/// Gets or sets the speed of the shake oscillation.
		/// Higher values result in faster shaking.
		/// </summary>
		/// <value>The shake frequency. Default is 10.</value>
		public float ShakeSpeed { get; set; }

		/// <summary>
		/// Internal cache for individual character strings.
		/// </summary>
		private FontStringCache Text { get; set; }

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="ShakyTextBuddy"/> class with default settings.
		/// Creates text that shakes up and down.
		/// </summary>
		public ShakyTextBuddy()
		{
			ShakeAmount = 10.0f;
			ShakeSpeed = 10.0f;
			Text = new FontStringCache();
		}

		/// <summary>
		/// Releases resources used by this instance.
		/// </summary>
		public override void Dispose()
		{
		}

		/// <summary>
		/// Renders text with a shaking/trembling effect to the screen.
		/// Each letter oscillates vertically with alternating letters moving in opposite directions.
		/// </summary>
		/// <param name="text">The text to render.</param>
		/// <param name="position">The position to render at. Interpretation depends on justification.</param>
		/// <param name="justification">The text justification mode.</param>
		/// <param name="scale">The scale multiplier for the font size.</param>
		/// <param name="color">The color to render the text in.</param>
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

			var fKerning = Font.Spacing * scale;

			//update the string cache
			Text.UpdateText(text);

			//Get the correct location
			var textSize = Font.MeasureString(text) * scale;

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
							position.X -= fKerning;
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
							position.X -= fKerning / 2.0f;
						}
					}
					break;
			}

			//ok, draw each individual letter
			for (int i = 0; i < Text.StringCache.Count; i++)
			{
				var pulsate = MathHelper.Clamp((ShakeAmount * (float)(Math.Sin(time.CurrentTime * ShakeSpeed))), -ShakeAmount, ShakeAmount);
				if ((i % 2) == 0)
				{
					pulsate *= -1.0f;
				}
				var pulsingPosition = position;
				pulsingPosition.Y += pulsate;

				var subString = Text.StringCache[i];

				//Clamp (because we dont want pure black and white)
				Font.DrawString(subString, pulsingPosition, scale, color, spriteBatch);

				position.X += Font.MeasureString(subString).X * scale;
				position.X += fKerning;
			}

			//return the end of that string
			return position.X + textSize.X;
		}

		#endregion //Methods
	}
}