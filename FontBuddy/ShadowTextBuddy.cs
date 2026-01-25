using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FontBuddyLib
{
	/// <summary>
	/// A text renderer that draws text with a drop shadow effect.
	/// The shadow is drawn behind the main text with configurable color, offset, and size.
	/// </summary>
	/// <remarks>
	/// The shadow effect is achieved by drawing the text twice - first the shadow, then the main text.
	/// This is a simple CPU-based approach without GPU shader effects.
	/// </remarks>
	public class ShadowTextBuddy : BaseFontBuddy, IShadowTextBuddy
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
		/// This is a multiplier of the font scale, not a point size.
		/// </summary>
		/// <value>The shadow size multiplier. Default is 1.05.</value>
		public float ShadowSize { get; set; }

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="ShadowTextBuddy"/> class with default settings.
		/// </summary>
		public ShadowTextBuddy()
		{
			ShadowColor = Color.Black;
			ShadowOffset = new Vector2(0.0f, 3.0f);
			ShadowSize = 1.05f;
		}

		/// <summary>
		/// Releases resources used by this instance.
		/// </summary>
		public override void Dispose()
		{
		}

		/// <summary>
		/// Renders text with a shadow effect to the screen.
		/// </summary>
		/// <param name="text">The text to render.</param>
		/// <param name="position">The position to render at. Interpretation depends on justification.</param>
		/// <param name="justification">The text justification mode.</param>
		/// <param name="scale">The scale multiplier for the font size.</param>
		/// <param name="color">The color to render the main text in.</param>
		/// <param name="spriteBatch">The SpriteBatch to use for rendering.</param>
		/// <param name="time">The game clock for time-based effects.</param>
		/// <returns>The X position at the end of the rendered text.</returns>
		public override float Write(string text,
			Vector2 position,
			Justify justification,
			float scale,
			Color color,
			SpriteBatch spriteBatch,
			GameClock time)
		{
			WriteShadow(text, position, justification, scale, spriteBatch, time);

			//draw my text
			return Font.Write(text, position, justification, scale, color, spriteBatch, time);
		}

		/// <summary>
		/// Draws the shadow portion of the text. Can be overridden in derived classes to customize shadow rendering.
		/// </summary>
		/// <param name="text">The text to draw as a shadow.</param>
		/// <param name="position">The base position for the text.</param>
		/// <param name="justification">The text justification mode.</param>
		/// <param name="scale">The scale multiplier for the font size.</param>
		/// <param name="spriteBatch">The SpriteBatch to use for rendering.</param>
		/// <param name="time">The game clock for time-based effects.</param>
		protected virtual void WriteShadow(string text,
			Vector2 position,
			Justify justification,
			float scale,
			SpriteBatch spriteBatch,
			GameClock time)
		{
			//Get the position of the shadow
			var shadowPos = position;
			switch (justification)
			{
				case Justify.Left:
					{
						Vector2 textSize = (!string.IsNullOrEmpty(text) ? (Font.MeasureString(text) * scale) : Vector2.Zero);
						shadowPos.X += (textSize.X / 2.0f);
					}
					break;
				case Justify.Right:
					{
						Vector2 textSize = (!string.IsNullOrEmpty(text) ? (Font.MeasureString(text) * scale) : Vector2.Zero);
						shadowPos.X -= (textSize.X / 2.0f);
					}
					break;
			}

			//draw the shadow
			Font.Write(text,
						shadowPos + ShadowOffset,
						Justify.Center,
						scale * ShadowSize,
						ShadowColor,
						spriteBatch,
						time);
		}
	}

	#endregion //Methods
}