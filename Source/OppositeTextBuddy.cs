using System;
using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

		/// <summary>
		/// write something on the screen
		/// </summary>
		/// <param name="strText">the text to write on the screen</param>
		/// <param name="Position">where to write at... either upper left, upper center, or upper right, depending on justication</param>
		/// <param name="eJustification">how to justify the text</param>
		/// <param name="fScale">how big to write.  This is not a point size to draw at, it is a multiple of the default font size!</param>
		/// <param name="myColor">color to draw the text... this will swap with the shadow color after a specified amount of time</param>
		/// <param name="mySpriteBatch">spritebatch to use to render the text</param>
		/// <param name="dTime">the current game time in seconds</param>
		public override float Write(string strText, Vector2 Position, Justify eJustification, float fScale, Color myColor,
									SpriteBatch mySpriteBatch, GameClock dTime)
		{
			float fKerning = Font.Spacing * fScale;

			//Get the correct location
			Vector2 textSize = Font.MeasureString(strText) * fScale;
			switch (eJustification)
			{
				case Justify.Right:
				{
					//move teh x value
					for (int i = 0; i < strText.Length; i++)
					{
						//get teh size of the character
						string strSubString = "" + strText[i];
						textSize = Font.MeasureString(strSubString) * fScale;
						Position.X -= textSize.X;

						//get the kerning too
						Position.X -= fKerning;
					}
				}
					break;

				case Justify.Center:
				{
					//move teh x value
					for (int i = 0; i < strText.Length; i++)
					{
						//get teh size of the character
						string strSubString = "" + strText[i];
						textSize = Font.MeasureString(strSubString) * fScale;
						Position.X -= (textSize.X / 2.0f);

						//get the kerning too
						Position.X -= fKerning / 2.0f;
					}
				}
					break;
			}

			//this is some shit we are gonna use to positaion a shadow
			Vector2 shadowPosition = Position;

			//draw the individual letter of the shadow first
			double dLetterTime = dTime.CurrentTime;
			for (int i = 0; i < strText.Length; i++)
			{
				//Add a tenth of a second for each letter in the string
				dLetterTime -= SwapSweep;
				float pulsate = MathHelper.Clamp((float)(Math.Sin(dLetterTime * SwapSpeed)), -0.5f, 0.5f);
				pulsate += 0.5f;
				string strSubString = "" + strText[i];

				//Clamp (because we dont want pure black and white)
				Color shadowColor = Color.Lerp(ShadowColor, myColor, pulsate);
				mySpriteBatch.DrawString(
					Font,
					strSubString,
					shadowPosition + ShadowOffset,
					shadowColor,
					0,
					Vector2.Zero,
					fScale * ShadowSize,
					SpriteEffects.None,
					0);

				shadowPosition.X += (Font.MeasureString(strSubString) * fScale).X;
				shadowPosition.X += fKerning;
			}

			dLetterTime = dTime.CurrentTime; //reset the time
			for (int i = 0; i < strText.Length; i++)
			{
				//draw the title
				dLetterTime -= SwapSweep;
				float pulsate = MathHelper.Clamp((float)(Math.Sin(dLetterTime * SwapSpeed)), -0.5f, 0.5f);
				pulsate += 0.5f;
				string strSubString = "" + strText[i];

				//get the opposite color of the shadow
				Color shadowColor = Color.Lerp(myColor, ShadowColor, pulsate);
				mySpriteBatch.DrawString(
					Font,
					strSubString,
					Position,
					shadowColor,
					0,
					Vector2.Zero,
					fScale,
					SpriteEffects.None,
					0);

				Position.X += (Font.MeasureString(strSubString) * fScale).X;
				Position.X += fKerning;
			}

			//return the end of that string
			return Position.X;
		}

		#endregion //Methods
	}
}