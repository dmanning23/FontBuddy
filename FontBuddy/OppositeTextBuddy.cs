using System;

namespace FontBuddy
{
	/// <summary>
	/// So what this one does:  Takes two colors, draws the shadow in one and text in the other.
	/// After a specified amount of time has passed, the colors wipe across and switch
	/// </summary>
	public class OppositeTextBuddy : FontBuddy
	{
		#region Fields
		
		/// <summary>
		/// How much the text shakes, higher values = more shake
		/// Defaults to 40
		/// </summary>
		/// <value>The shake amount.</value>
		public float ShakeAmount { get; set; }
		
		/// <summary>
		/// How fast to shake the text
		/// defaults to 10
		/// </summary>
		/// <value>The shake speed.</value>
		public float ShakeSpeed { get; set; }
		
		#endregion //Fields

		#region Methods

		public OppositeTextBuddy ()
		{
		}

		public override void DrawText(GameTime gameTime)
		{
			SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
			spriteBatch.Begin();
			SpriteFont font = ScreenManager.MenuTitleFont;
			
			//Get the game time in seconds
			double time = gameTime.TotalGameTime.TotalSeconds;
			
			//Draw the game title!
			
			string strTitle1 = "Opposites";
			
			Vector2 Origin = new Vector2(0.0f, 0.0f);
			
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
				
				//Add a tenth of a second for each letter in the string
				time -= (((double)i) * 0.025);
				float pulsate = MathHelper.Clamp((0.75f * (float)(Math.Sin(time * 1.0f))), -0.5f, 0.5f);
				string strSubString = "" + strTitle1[i];
				
				//Clamp (because we dont want pure black and white)
				float fShadowColorValue = MathHelper.Clamp((pulsate * -1.0f) + 0.5f, 0.15f, .85f);
				Color myShadowColor = new Color(fShadowColorValue, fShadowColorValue, fShadowColorValue, TransitionAlpha);
				spriteBatch.DrawString(
					font,
					strSubString,
					shadowPosition,
					myShadowColor,
					0,
					Origin,
					shadowScale,
					SpriteEffects.None,
					0);
				
				shadowPosition.X += (font.MeasureString(strSubString) * shadowScale).X;
			}
			
			time = gameTime.TotalGameTime.TotalSeconds; //reset the time
			for (int i = 0; i < strTitle1.Length; i++)
			{
				//draw the title
				
				time -= (((double)i) * 0.025);
				float pulsate = MathHelper.Clamp((0.75f * (float)(Math.Sin(time * 1.0f))), -0.5f, 0.5f);
				string strSubString = "" + strTitle1[i];
				
				float fColorValue = MathHelper.Clamp(pulsate + 0.5f, 0.15f, .85f);
				Color myColor = new Color(fColorValue, fColorValue, fColorValue, TransitionAlpha);
				spriteBatch.DrawString(
					font,
					strSubString,
					titlePosition,
					myColor,
					0,
					Origin,
					titleScale,
					SpriteEffects.None,
					0);
				
				titlePosition.X += (font.MeasureString(strSubString) * titleScale).X;
			}
			
			spriteBatch.End();
		}

		#endregion //Methods
	}
}

