using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FontBuddyLib
{
	/// <summary>
	/// This dude takes a list of colors and cycles the color of each letter
	/// </summary>
	public class RainbowTextBuddy : ShadowTextBuddy
	{
		#region Members

		/// <summary>
		/// How fast to change the color
		/// Defaults to 2.0
		/// </summary>
		public float RainbowSpeed { get; set; }

		#endregion //Members

		#region Properties

		/// <summary>
		/// The list of colors to cycles through
		/// </summary>
		public List<Color> Colors { get; set; }

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Cosntructor!
		/// </summary>
		public RainbowTextBuddy()
		{
			RainbowSpeed = 2.0f;
			Colors = new List<Color>();

			//well, this is the rainbow buddy, put some shit in there
			Colors.Add(Color.Purple);
			Colors.Add(Color.Blue);
			Colors.Add(Color.Green);
			Colors.Add(Color.Yellow);
			Colors.Add(Color.Orange);
			Colors.Add(Color.Red);
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
		public override float Write(string strText, 
			Vector2 Position, 
			Justify eJustification, 
			float fScale, 
			Color myColor,
			SpriteBatch mySpriteBatch, 
			double dTime)
		{
			//First draw the shadow
			ShadowWriter.Write(strText,
			                   Position + ShadowOffset,
			                   eJustification,
			                   fScale * ShadowSize,
			                   ShadowColor,
			                   mySpriteBatch,
			                   dTime);

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

			dTime *= RainbowSpeed;
			dTime += strText.Length;
			for (int i = 0; i < strText.Length; i++)
			{
				//draw the title

				//Get the current color
				dTime -= 0.6;
				int iIndex = (int)dTime % Colors.Count;

				//get the next color
				int iNextIndex = iIndex + 1;
				if (iNextIndex >= Colors.Count)
				{
					iNextIndex = 0;
				}

				//get the ACTUAL lerped color of this letter
				var fRemainder = (float)(dTime - (int)dTime);
				Color letterColor = Color.Lerp(Colors[iIndex], Colors[iNextIndex], fRemainder);

				string strSubString = "" + strText[i];
				mySpriteBatch.DrawString(
					Font,
					strSubString,
					Position,
					letterColor,
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