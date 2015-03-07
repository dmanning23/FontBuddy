using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FontBuddyLib
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
	public class FontBuddy : IFontBuddy
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
		}

		/// <summary>
		/// given a content manager and a resource name, load the resource as a bitmap font
		/// </summary>
		/// <param name="rContent"></param>
		/// <param name="strResource"></param>
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
		/// <param name="position">where to write at... either upper left, upper center, or upper right, depending on justication</param>
		/// <param name="eJustification">how to justify the text</param>
		/// <param name="fScale">how big to write.  This is not a point size to draw at, it is a multiple of the default font size!</param>
		/// <param name="myColor">the color to draw the text</param>
		/// <param name="mySpriteBatch">spritebatch to use to render the text</param>
		/// <param name="dTime">Most of the other font buddy classes use time somehow, but can jsut send in 0.0f for this dude or ignoer it</param>
		public virtual float Write(string strText,
			Vector2 position, 
			Justify eJustification, 
			float fScale, 
			Color myColor,
			SpriteBatch mySpriteBatch, 
			double dTime = 0.0f)
		{
			return DrawText(strText, position, eJustification, fScale, myColor, mySpriteBatch, dTime);
		}

		/// <summary>
		/// write something on the screen
		/// </summary>
		/// <param name="strText">the text to write on the screen</param>
		/// <param name="position">where to write at... either upper left, upper center, or upper right, depending on justication</param>
		/// <param name="eJustification">how to justify the text</param>
		/// <param name="fScale">how big to write.  This is not a point size to draw at, it is a multiple of the default font size!</param>
		/// <param name="myColor">the color to draw the text</param>
		/// <param name="mySpriteBatch">spritebatch to use to render the text</param>
		/// <param name="dTime">Most of the other font buddy classes use time somehow, but can jsut send in 0.0f for this dude or ignoer it</param>
		public virtual float Write(string strText,
			Point position, 
			Justify eJustification, 
			float fScale, 
			Color myColor,
			SpriteBatch mySpriteBatch, 
			double dTime = 0.0f)
		{
			return Write(strText, new Vector2(position.X, position.Y), eJustification, fScale, myColor, mySpriteBatch, dTime);
		}

		/// <summary>
		/// write something on the screen
		/// </summary>
		/// <param name="strText">the text to write on the screen</param>
		/// <param name="position">where to write at... either upper left, upper center, or upper right, depending on justication</param>
		/// <param name="eJustification">how to justify the text</param>
		/// <param name="fScale">how big to write.  This is not a point size to draw at, it is a multiple of the default font size!</param>
		/// <param name="myColor">the color to draw the text</param>
		/// <param name="mySpriteBatch">spritebatch to use to render the text</param>
		/// <param name="dTime">Most of the other font buddy classes use time somehow, but can jsut send in 0.0f for this dude or ignoer it</param>
		protected float DrawText(string strText, 
			Vector2 position, 
			Justify eJustification, 
			float fScale, 
			Color myColor,
			SpriteBatch mySpriteBatch, 
			double dTime = 0.0f)
		{
			Debug.Assert(null != Font);

			//if this thing is empty, dont do anything
			if (string.IsNullOrEmpty(strText))
			{
				return position.X;
			}

			position = JustifiedPosition(strText, position, eJustification, fScale);

			//okay, draw the actual string
			mySpriteBatch.DrawString(Font,
			                         strText,
									 position,
			                         myColor,
			                         0.0f,
			                         Vector2.Zero,
			                         fScale,
			                         SpriteEffects.None,
			                         0);

			//return the end of that string
			return position.X + (Font.MeasureString(strText).X * fScale);
		}

		protected Vector2 JustifiedPosition(string strText, Vector2 position, Justify eJustification, float fScale)
		{
			//Get the correct location
			Vector2 textSize = (!string.IsNullOrEmpty(strText) ? (Font.MeasureString(strText) * fScale) : Vector2.Zero);

			switch (eJustification)
			{
					//left = use teh x value (no cahnge)

				case Justify.Right:
				{
					//move teh x value
					position.X -= textSize.X;
				}
					break;

				case Justify.Center:
				{
					//move teh x value
					position.X -= (textSize.X / 2.0f);
				}
					break;
			}

			return position;
		}

		#endregion //Methods
	}
}