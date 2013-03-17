using System;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FontBuddy
{
	/// <summary>
	/// This dude draw some text with a shadow.  You can change some stuff like shadow distance, color
	/// It not that fancy like at all, it just draw the text twice... there's no gpu magic or nothing :P
	/// </summary>
	public class ShadowTextBuddy : FontBuddy
	{
		#region Members

		/// <summary>
		/// font buddy we are going to use to draw the shadow
		/// </summary>
		private FontBuddy _shadowWriter = new FontBuddy();

		/// <summary>
		/// color to draw the shadow
		/// </summary>
		public Color ShadowColor { get; set; }

		/// <summary>
		/// The font this dude is "helping" with... overridden here so the shadow text uses the same font
		/// </summary>
		public override SpriteFont Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				_shadowWriter.Font = value;
				base.Font = value;
			}
		}

		/// <summary>
		/// offset of our text to draw the shadow
		/// </summary>
		public Vector2 ShadowOffset { get; set; }

		/// <summary>
		/// How much bigger than our text to make the shadow... this is a multiplier of the font size, NOT A POINT SIZE!!!
		/// </summary>
		public float ShadowSize { get; set; }

		#endregion //Members

		#region Properties

		protected FontBuddy ShadowWriter
		{
			get { return _shadowWriter; }
		}

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Constructor!
		/// </summary>
		public ShadowTextBuddy()
			: base()
		{
			ShadowColor = Color.DarkGray;
			ShadowOffset = new Vector2(0.0f, 50.0f);
			ShadowSize = 1.1f;
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
		public override float Write(string strText, Vector2 Position, Justify eJustification, float fScale, Color myColor, SpriteBatch mySpriteBatch, double dTime = 0.0f)
		{
			//darw the shadow
			_shadowWriter.Write(strText,
				Position + ShadowOffset, 
				eJustification, 
				fScale * ShadowSize, 
				ShadowColor, 
				mySpriteBatch, 
				dTime);

			//draw my text
			return base.Write(strText, Position, eJustification, fScale, ShadowColor, mySpriteBatch, dTime);
		}

		#endregion //Methods
	}
}

