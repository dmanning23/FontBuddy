using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FontBuddyLib
{
	/// <summary>
	/// A text renderer that creates an animated color-swapping effect between the text and its shadow.
	/// The text and shadow colors smoothly swap in a wave pattern across the letters.
	/// </summary>
	/// <remarks>
	/// This effect draws each letter individually, with the text and shadow colors interpolating
	/// between each other based on a sine wave. The effect sweeps across the text from left to right.
	/// </remarks>
	public class OppositeTextBuddy : BaseFontBuddy, IShadowTextBuddy
	{
		#region Properties

		/// <summary>
		/// Gets or sets the color used for the shadow (also swapped with the text color).
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
		/// Gets or sets the speed of the color swapping animation.
		/// Higher values result in faster color transitions.
		/// </summary>
		/// <value>The swap animation speed. Default is 2.0.</value>
		public float SwapSpeed { get; set; }

		/// <summary>
		/// Gets or sets the time delay between each letter's color swap in the wave effect.
		/// </summary>
		/// <value>The sweep delay per character. Default is 0.1.</value>
		public float SwapSweep { get; set; }

		/// <summary>
		/// Internal cache for individual character strings.
		/// </summary>
		private FontStringCache Text { get; set; }

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="OppositeTextBuddy"/> class with default settings.
		/// </summary>
		public OppositeTextBuddy()
		{
			ShadowColor = Color.Black;
			ShadowOffset = new Vector2(0.0f, 3.0f);
			ShadowSize = 1.05f;

			SwapSpeed = 2.0f;
			SwapSweep = 0.1f;
			Text = new FontStringCache();
		}

		/// <summary>
		/// Releases resources used by this instance.
		/// </summary>
		public override void Dispose()
		{
		}

		/// <summary>
		/// Renders text with an animated color-swapping effect between text and shadow colors.
		/// </summary>
		/// <param name="text">The text to render.</param>
		/// <param name="position">The position to render at. Interpretation depends on justification.</param>
		/// <param name="justification">The text justification mode.</param>
		/// <param name="scale">The scale multiplier for the font size.</param>
		/// <param name="color">The primary text color (will swap with shadow color).</param>
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

			Text.UpdateText(text);

			float fKerning = Font.Spacing * scale;

			//Get the correct location
			Vector2 textSize = Font.MeasureString(text) * scale;
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

			//this is some shit we are gonna use to positaion a shadow
			var shadowPosition = position;

			//draw the individual letter of the shadow first
			var letterTime = time.CurrentTime;
			for (var i = 0; i < Text.StringCache.Count; i++)
			{
				//Add a tenth of a second for each letter in the string
				letterTime -= SwapSweep;
				var pulsate = MathHelper.Clamp((float)(Math.Sin(letterTime * SwapSpeed)), -0.5f, 0.5f);
				pulsate += 0.5f;
				var subString = Text.StringCache[i];

				//Clamp (because we dont want pure black and white)
				Font.DrawString(subString, shadowPosition + ShadowOffset, scale * ShadowSize, Color.Lerp(ShadowColor, color, pulsate), spriteBatch);

				shadowPosition.X += (Font.MeasureString(subString) * scale).X;
				shadowPosition.X += fKerning;
			}

			letterTime = time.CurrentTime; //reset the time
			for (var i = 0; i < Text.StringCache.Count; i++)
			{
				//draw the title
				letterTime -= SwapSweep;
				var pulsate = MathHelper.Clamp((float)(Math.Sin(letterTime * SwapSpeed)), -0.5f, 0.5f);
				pulsate += 0.5f;
				var subString = Text.StringCache[i];

				//get the opposite color of the shadow
				Font.DrawString(subString, position, scale, Color.Lerp(color, ShadowColor, pulsate), spriteBatch);

				position.X += (Font.MeasureString(subString) * scale).X;
				position.X += fKerning;
			}

			//return the end of that string
			return position.X;
		}

		#endregion //Methods
	}
}