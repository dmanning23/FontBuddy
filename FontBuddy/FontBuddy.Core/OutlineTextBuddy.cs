using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FontBuddyLib
{
	/// <summary>
	/// This dude draw some text with an outline.
	/// </summary>
	public class OutlineTextBuddy : BaseFontBuddy
	{
		#region Properties

		/// <summary>
		/// color to draw the outline
		/// </summary>
		public Color OutlineColor { get; set; }

		/// <summary>
		/// offset of our text to draw the shadow
		/// defaults to 0.0f, 3.0f
		/// </summary>
		public Vector2 ShadowOffset { get; set; }

		/// <summary>
		/// How much bigger than our text to make the outline... this is a point size, NOT A MUTLIPLIER
		/// </summary>
		public int OutlineSize { get; set; }

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Constructor!
		/// </summary>
		public OutlineTextBuddy()
		{
			OutlineColor = Color.Black;
			ShadowOffset = new Vector2(0.0f, 3.0f);
			OutlineSize = 5;
		}

		public override void Dispose()
		{
		}

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