using System;

namespace FontBuddy
{
	public class PulsateBuddy : ShadowTextBuddy
	{
		public PulsateBuddy()
		{
		}

			/// <summary>
		/// Draws the menu entry. This can be overridden to customize the appearance.
		/// </summary>
		public virtual void Draw(CMenuScreen screen, Vector2 position, bool isSelected, GameTime gameTime)
		{
			// Draw text, centered on the middle of each line.
			CScreenManager screenManager = screen.ScreenManager;
			SpriteBatch spriteBatch = screenManager.SpriteBatch;
			SpriteFont font = screenManager.Font;

			// Pulsate the size of the selected menu entry.
			double time = gameTime.TotalGameTime.TotalSeconds;
			float pulsate = (float)(Math.Sin(time * 4.0) + 1.0);
			float scale = 1 + pulsate * 0.15f * m_fSelectionFade;

			// Draw the selected entry in yellow, otherwise white.
			Color color = isSelected ? Color.Red : Color.White;

			// Modify the alpha to fade text out during transitions.
			Vector3 myColor = color.ToVector3();
			color = new Color(myColor.X, myColor.Y, myColor.Z, screen.TransitionAlpha);
			myColor = Color.Black.ToVector3();
			Color backgroundColor = new Color(myColor.X, myColor.Y, myColor.Z, screen.TransitionAlpha);

			Vector2 origin = new Vector2(0, font.LineSpacing / 2);

			//First draw the menu item in black, which will add a nice shadow effect to selected items
			spriteBatch.DrawString(font, m_strText, position, backgroundColor, 0, origin, SizeMultiplier, SpriteEffects.None, 0);

			//adjust the position to account for the pulsating
			float fAdjust = ((font.MeasureString(Text).X * SizeMultiplier * scale) - (font.MeasureString(Text).X * SizeMultiplier)) / 2.0f;
			position.X -= fAdjust;

			//Draw the menu item
			spriteBatch.DrawString(font, m_strText, position, color, 0, origin, scale * SizeMultiplier, SpriteEffects.None, 0);
		}
	}
}

