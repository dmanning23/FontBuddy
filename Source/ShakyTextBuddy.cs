using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FontBuddyLib
{
	public class ShakyTextBuddy : FontBuddy
	{
		#region Fields

		/// <summary>
		/// How much the text shakes, higher values = more shake
		/// Defaults to 10
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

		/// <summary>
		/// Initializes a new instance of the <see cref="FontBuddy.ShakyTextBuddy"/> class.
		/// Draw some text that shakes up and down!
		/// </summary>
		public ShakyTextBuddy()
		{
			ShakeAmount = 10.0f;
			ShakeSpeed = 10.0f;
		}

		/// <summary>
		/// write something on the screen
		/// </summary>
		/// <param name="strText">the text to write on the screen</param>
		/// <param name="Position">where to write at... either upper left, upper center, or upper right, depending on justication</param>
		/// <param name="eJustification">how to justify the text</param>
		/// <param name="fScale">how big to write.  This is not a point size to draw at, it is a multiple of the default font size!</param>
		/// <param name="myColor">the color to draw the text</param>
		/// <param name="mySpriteBatch">spritebatch to use to render the text</param>
		/// <returns>float: the x location of the end of the text.</returns>
		/// <param name="dTime">The current time in seconds</param>
		public override float Write(string strText, Vector2 Position, Justify eJustification, float fScale, Color myColor,
		                            SpriteBatch mySpriteBatch, double dTime)
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

			//ok, draw each individual letter
			for (int i = 0; i < strText.Length; i++)
			{
				float pulsate = MathHelper.Clamp((ShakeAmount * (float)(Math.Sin(dTime * ShakeSpeed))), -ShakeAmount, ShakeAmount);
				if ((i % 2) == 0)
				{
					pulsate *= -1.0f;
				}
				Vector2 PulsingPosition = Position;
				PulsingPosition.Y += pulsate;

				string strSubString = "" + strText[i];

				//Clamp (because we dont want pure black and white)
				mySpriteBatch.DrawString(
					Font,
					strSubString,
					PulsingPosition,
					myColor,
					0,
					Vector2.Zero,
					fScale,
					SpriteEffects.None,
					0);

				Position.X += Font.MeasureString(strSubString).X * fScale;
				Position.X += fKerning;
			}

			//return the end of that string
			return Position.X + textSize.X;
		}

		#endregion //Methods
	}
}