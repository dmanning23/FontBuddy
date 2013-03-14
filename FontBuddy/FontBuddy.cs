using System;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FontBuddy
{
	//how to justify the font writing
	public enum Justify
	{
		Left,
		Right,
		Center
	}

	/// <summary>
	/// This is just a simple class for drawing text on the screen.
	/// Because that is just way harder in XNA than it needs to be!
	/// </summary>
	public class FontBuddy

	{
		#region Properties

		/// <summary>
		/// The font this dude is "helping" with
		/// </summary>
		public SpriteFont Font { get; set; }

		#endregion //Properties

		#region Methods

		/// <summary>
		/// constructor... you need to set the font or call load content if you use this one
		/// </summary>
		public FontBuddy()
		{
			Font = null;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="rFont">you loaded a font yourself!!!</param>
		public FontBuddy(SpriteFont rFont)
		{
			Font = rFont;
		}

		public void LoadContent(ContentManager rContent, string strResource)
		{
			//load font
			if (null == Font)
			{
				Font = rContent.Load<SpriteFont>(strResource);
			}
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
		public float Write(string strText, Vector2 Position, Justify eJustification, float fScale, Color myColor, SpriteBatch mySpriteBatch)
		{
			Debug.Assert(null != Font);

			//Get the correct location
			Vector2 textPosition = Position;
			Vector2 textSize = Font.MeasureString(strText) * fScale;
			
			//set teh y location
			textPosition.Y -= textSize.Y * 0.5f;
			//textPosition.Y += m_Font.LineSpacing;
			
			switch (eJustification)
			{
				case Justify.Left:
				{
					//use teh x value (no cahnge)
				}
				break;

				case Justify.Right:
				{
					//move teh x value
					textPosition.X -= textSize.X;
				}
				break;

				case Justify.Center:
				{
					//move teh x value
					textPosition.X -= (textSize.X / 2.0f);
				}
				break;

				default:
				{
					//wtf did you do?
					Debug.Assert(false);
				}
				break;
			}

			//okay, draw the actual string
			mySpriteBatch.DrawString(Font, strText, textPosition, myColor, 0,
				Vector2.Zero,
				fScale,
				SpriteEffects.None,
				0);

			//return the end of that string
			return textPosition.X + textSize.X;
		}

		/// <summary>
		/// Draw some text, but do it as super cool SHAKY text
		/// </summary>
		/// <param name="strText">the text to write on the screen</param>
		/// <param name="Position">where to write at... either upper left, upper center, or upper right, depending on justication</param>
		/// <param name="eJustification">how to justify the text</param>
		/// <param name="fScale">how big to write.  This is not a point size to draw at, it is a multiple of the default font size!</param>
		/// <param name="myColor">the color to draw the text</param>
		/// <param name="mySpriteBatch">spritebatch to use to render the text</param>
		/// <param name="dTime">the current time, used to update the shakiness</param>
		public void WriteShaky(string strText, Vector2 Position, Justify eJustification, Vector2 fScale, Color myColor, SpriteBatch mySpriteBatch, double dTime)
		{
			//Get the correct location
			Vector2 textPosition = Position;
			Vector2 textSize = Font.MeasureString(strText) * fScale;

			//set teh y location
			textPosition.Y -= textSize.Y * 0.5f;

			float fKerning = (Font.MeasureString(" ").X * 0.25f) * fScale.X;

			switch (eJustification)
			{
				case Justify.Left:
					{
						//use teh x value (no cahnge)
					}
					break;

				case Justify.Right:
					{
						//move teh x value
						for (int i = 0; i < strText.Length; i++)
						{
							//get teh size of the character
							StringBuilder strSubString = new StringBuilder(strText[i]);
							textSize = Font.MeasureString(strSubString.ToString()) * fScale;
							textPosition.X -= textSize.X;

							//get the kerning too
							textPosition.X -= fKerning;
						}
					}
					break;

				case Justify.Center:
					{
						//move teh x value
						for (int i = 0; i < strText.Length; i++)
						{
							//get teh size of the character
							StringBuilder strSubString = new StringBuilder(strText[i]);
							textSize = Font.MeasureString(strSubString.ToString()) * fScale;
							textPosition.X -= (textSize.X / 2.0f);

							//get the kerning too
							textPosition.X -= fKerning / 2.0f;
						}
					}
					break;

				default:
					{
						//wtf did you do?
						Debug.Assert(false);
					}
					break;
			}

			//TODO: these hard coded values can be adjusted to change the shakiness

			//ok, draw each individual letter
			for (int i = 0; i < strText.Length; i++)
			{
				float pulsate = MathHelper.Clamp((40.0f * (float)(Math.Sin(dTime * 10.0f))), -40.0f, 40.0f);
				if ((i % 2) == 0)
				{
					pulsate *= -1.0f;
				}
				Vector2 PulsingPosition = textPosition;
				PulsingPosition.Y += pulsate;

				StringBuilder strSubString = new StringBuilder(strText[i]);

				//Clamp (because we dont want pure black and white)
				mySpriteBatch.DrawString(
					Font,
					strSubString.ToString(),
					PulsingPosition,
					myColor,
					0,
					Vector2.Zero,
					fScale,
					SpriteEffects.None,
					0);

				textPosition.X += Font.MeasureString(strSubString.ToString()).X * fScale.X;
				textPosition.X += fKerning;
			}
		}

		#endregion //Methods
	}
}