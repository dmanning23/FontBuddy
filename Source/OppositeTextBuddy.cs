using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FontBuddyLib
{
	/// <summary>
	/// So what this one does:  Takes two colors, draws the shadow in one and text in the other.
	/// After a specified amount of time has passed, the colors wipe across and switch
	/// </summary>
	public class OppositeTextBuddy : ShadowTextBuddy
	{
		#region Fields

		/// <summary>
		/// how fast to swap colors... defaults to 2.0f
		/// </summary>
		public float SwapSpeed { get; set; }

		/// <summary>
		/// How often to swap colors... defaults to 0.1f
		/// </summary>
		public float SwapSweep { get; set; }

		#endregion //Fields

		#region Methods

		/// <summary>
		/// Constructor
		/// </summary>
		public OppositeTextBuddy()
		{
			SwapSpeed = 2.0f;
			SwapSweep = 0.1f;
		}

		public override float Write(string text,
			Vector2 position,
			Justify justification,
			float scale,
			Color color,
			SpriteBatch spriteBatch,
			GameClock time)
		{
			float fKerning = Font.Spacing * scale;

			//Get the correct location
			Vector2 textSize = Font.MeasureString(text) * scale;
			switch (justification)
			{
				case Justify.Right:
				{
					//move teh x value
					for (int i = 0; i < text.Length; i++)
					{
						//get teh size of the character
						string strSubString = "" + text[i];
						textSize = Font.MeasureString(strSubString) * scale;
						position.X -= textSize.X;

						//get the kerning too
						position.X -= fKerning;
					}
				}
				break;

				case Justify.Center:
				{
					//move teh x value
					for (int i = 0; i < text.Length; i++)
					{
						//get teh size of the character
						string strSubString = "" + text[i];
						textSize = Font.MeasureString(strSubString) * scale;
						position.X -= (textSize.X / 2.0f);

						//get the kerning too
						position.X -= fKerning / 2.0f;
					}
				}
				break;
			}

			//this is some shit we are gonna use to positaion a shadow
			Vector2 shadowPosition = position;

			//draw the individual letter of the shadow first
			double dLetterTime = time.CurrentTime;
			for (int i = 0; i < text.Length; i++)
			{
				//Add a tenth of a second for each letter in the string
				dLetterTime -= SwapSweep;
				float pulsate = MathHelper.Clamp((float)(Math.Sin(dLetterTime * SwapSpeed)), -0.5f, 0.5f);
				pulsate += 0.5f;
				string strSubString = "" + text[i];

				//Clamp (because we dont want pure black and white)
				Color shadowColor = Color.Lerp(ShadowColor, color, pulsate);
				spriteBatch.DrawString(
					Font,
					strSubString,
					shadowPosition + ShadowOffset,
					shadowColor,
					0,
					Vector2.Zero,
					scale * ShadowSize,
					SpriteEffects.None,
					0);

				shadowPosition.X += (Font.MeasureString(strSubString) * scale).X;
				shadowPosition.X += fKerning;
			}

			dLetterTime = time.CurrentTime; //reset the time
			for (int i = 0; i < text.Length; i++)
			{
				//draw the title
				dLetterTime -= SwapSweep;
				float pulsate = MathHelper.Clamp((float)(Math.Sin(dLetterTime * SwapSpeed)), -0.5f, 0.5f);
				pulsate += 0.5f;
				string strSubString = "" + text[i];

				//get the opposite color of the shadow
				Color shadowColor = Color.Lerp(color, ShadowColor, pulsate);
				spriteBatch.DrawString(
					Font,
					strSubString,
					position,
					shadowColor,
					0,
					Vector2.Zero,
					scale,
					SpriteEffects.None,
					0);

				position.X += (Font.MeasureString(strSubString) * scale).X;
				position.X += fKerning;
			}

			//return the end of that string
			return position.X;
		}

		#endregion //Methods
	}
}