using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FontBuddyLib
{
	/// <summary>
	/// A text renderer that draws text with an outline/stroke effect.
	/// The outline is created by drawing the text multiple times at offset positions.
	/// </summary>
	/// <remarks>
	/// The outline effect is achieved by drawing the text at multiple offset positions around the main text,
	/// then drawing the main text on top. This creates a clean outline effect at the edges of the text.
	/// </remarks>
	public class OutlineTextBuddy : BaseFontBuddy
	{
		#region Properties

		/// <summary>
		/// Gets or sets the color used to draw the text outline.
		/// </summary>
		/// <value>The outline color. Default is <see cref="Color.Black"/>.</value>
		public Color OutlineColor { get; set; }

		/// <summary>
		/// Gets or sets the pixel offset for shadow effects (inherited from shadow functionality).
		/// </summary>
		/// <value>The shadow offset as a <see cref="Vector2"/>. Default is (0, 3).</value>
		public Vector2 ShadowOffset { get; set; }

		/// <summary>
		/// Gets or sets the size of the outline in pixels.
		/// This is an absolute pixel value, not a scale multiplier.
		/// </summary>
		/// <value>The outline size in pixels. Default is 5.</value>
		public int OutlineSize { get; set; }

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="OutlineTextBuddy"/> class with default settings.
		/// </summary>
		public OutlineTextBuddy()
		{
			OutlineColor = Color.Black;
			ShadowOffset = new Vector2(0.0f, 3.0f);
			OutlineSize = 5;
		}

		/// <summary>
		/// Releases resources used by this instance.
		/// </summary>
		public override void Dispose()
		{
		}

		/// <summary>
		/// Renders text with an outline effect to the screen.
		/// The outline is drawn first at multiple offset positions, then the main text is drawn on top.
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
			float alpha = ((float)(OutlineColor.A * color.A) / 65025.0f);
#if !XNA
			Color backColor = new Color(OutlineColor, alpha);
#else
			Color backColor = OutlineColor;
			backColor.A = color.A;
#endif

			//draw the outline
			for (float x = -OutlineSize; x <= OutlineSize; x += OutlineSize / 2f)
			{
				for (float y = -OutlineSize; y <= OutlineSize; y += OutlineSize / 2f)
				{
					bool xEdge = ((x == -OutlineSize) || (x == OutlineSize));
					bool yEdge = ((y == -OutlineSize) || (y == OutlineSize));
					if (!xEdge && !yEdge)
					{
						continue;
					}

					Vector2 outlinePos = new Vector2(position.X + x, position.Y + y);
					Font.Write(text,
								outlinePos,
								justification,
								scale,
								backColor,
								spriteBatch,
								time);
				}
			}

			//draw my text
			return Font.Write(text, position, justification, scale, color, spriteBatch, time);
		}

		#endregion //Methods
	}
}