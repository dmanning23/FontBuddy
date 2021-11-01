using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FontBuddyLib
{
	/// <summary>
	/// This dude draw some text with a shadow.  You can change some stuff like shadow distance, color
	/// It not that fancy like at all, it just draw the text twice... there's no gpu magic or nothing :P
	/// </summary>
	public class ShadowTextBuddy : BaseFontBuddy, IShadowTextBuddy
	{
		#region Properties

		/// <summary>
		/// color to draw the shadow
		/// </summary>
		public Color ShadowColor { get; set; }

		/// <summary>
		/// offset of our text to draw the shadow
		/// defaults to 0.0f, 3.0f
		/// </summary>
		public Vector2 ShadowOffset { get; set; }

		/// <summary>
		/// How much bigger than our text to make the shadow... this is a multiplier of the font size, NOT A POINT SIZE!!!
		/// defaults to 1.05f
		/// </summary>
		public float ShadowSize { get; set; }

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Constructor!
		/// </summary>
		public ShadowTextBuddy()
		{
			ShadowColor = Color.Black;
			ShadowOffset = new Vector2(0.0f, 3.0f);
			ShadowSize = 1.05f;
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
			WriteShadow(text, position, justification, scale, spriteBatch, time);

			//draw my text
			return Font.Write(text, position, justification, scale, color, spriteBatch, time);
		}

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