using System;

namespace FontBuddy
{
	public class RainbowTextBuddy : ShadowTextBuddy
	{
		public RainbowTextBuddy()
		{
		}

			/// <summary>
		/// Draws the background screen.
		/// </summary>
		public override void Draw(GameTime gameTime)
		{
			//Get the game time in seconds
			double time = gameTime.TotalGameTime.TotalSeconds;

			//Draw the game title!

			//the colors used to draw title
			Color dark = new Color(0.156f, 0.156f, 0.156f, TransitionAlpha);

			string strTitle1 = "Pajamorama";

			//position of the game title
			Vector2 titleScale = new Vector2(0.0f);
			titleScale.X = 1.5f;
			titleScale.Y = 1.45f;
			float fTitlePositionX = ScreenRect.Center.X - (font.MeasureString(strTitle1) * titleScale / 2.0f).X;
			float fTitlePositionY = ScreenRect.Center.Y - ((font.MeasureString(strTitle1) * titleScale).Y * 1.4f);
			Vector2 titlePosition = new Vector2(fTitlePositionX, fTitlePositionY);

			//position of the shadow
			Vector2 shadowScale = new Vector2(0.0f);
			shadowScale.X = 1.45f;
			shadowScale.Y = 1.4f;
			float fShadowPositionX = ScreenRect.Center.X - (font.MeasureString(strTitle1) * shadowScale / 2.0f).X;
			float fShadowPositionY = ScreenRect.Center.Y - ((font.MeasureString(strTitle1) * shadowScale).Y * 1.4f);
			Vector2 shadowPosition = new Vector2(fShadowPositionX, fShadowPositionY);

			//draw each individual letter of the title
			for (int i = 0; i < strTitle1.Length; i++)
			{
				//draw the shadow
				string strSubString = "" + strTitle1[i];
				spriteBatch.DrawString(
					font,
					strSubString,
					shadowPosition,
					dark,
					0,
					Vector2.Zero,
					shadowScale,
					SpriteEffects.None,
					0);

				shadowPosition.X += (font.MeasureString(strSubString) * shadowScale).X;
			}

			time = gameTime.TotalGameTime.TotalSeconds; //reset the time
			time *= 2.0; //TODO: change this number to speed up/slow down color change
			time += strTitle1.Length;
			for (int i = 0; i < strTitle1.Length; i++)
			{
				//draw the title

				//Get the color
				Color letterColor = Color.White;
				time -= 0.6;
				int iIndex = (int)time % 6; //TODO: change 6 to be the number of colors we are cycling through
				float fRemainder = (float)(time - (int)time);

				switch (iIndex)
				{
					case 0:
					{
						letterColor = Color.Lerp(Color.Red, Color.Purple, fRemainder);
					}
					break;
					case 1:
					{
						letterColor = Color.Lerp(Color.Purple, Color.Blue, fRemainder);
					}
					break;
					case 2:
					{
						letterColor = Color.Lerp(Color.Blue, Color.Green, fRemainder);
					}
					break;
					case 3:
					{
						letterColor = Color.Lerp(Color.Green, Color.Yellow, fRemainder);
					}
					break;
					case 4:
					{
						letterColor = Color.Lerp(Color.Yellow, Color.Orange, fRemainder);
					}
					break;
					case 5:
					{
						letterColor = Color.Lerp(Color.Orange, Color.Red, fRemainder);
					}
					break;
				}
				
				string strSubString = "" + strTitle1[i];

				spriteBatch.DrawString(
					font,
					strSubString,
					titlePosition,
					FadeAlphaDuringTransition(letterColor),
					0,
					Vector2.Zero,
					titleScale,
					SpriteEffects.None,
					0);

				titlePosition.X += (font.MeasureString(strSubString) * titleScale).X;
			}
		}
	}
}

