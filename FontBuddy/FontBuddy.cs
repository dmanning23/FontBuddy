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
		public virtual SpriteFont Font { get; set; }

		#endregion //Properties

		#region Methods

		/// <summary>
		/// constructor... you need to set the font or call load content if you use this one
		/// </summary>
		public FontBuddy()
		{
			Font = null;
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
		/// <param name="dTime">Most of the other font buddy classes use time somehow, but can jsut send in 0.0f for this dude or ignoer it</param>
		public virtual float Write(string strText, Vector2 Position, Justify eJustification, float fScale, Color myColor, SpriteBatch mySpriteBatch, double dTime = 0.0f)
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

		#endregion //Methods
	}
}