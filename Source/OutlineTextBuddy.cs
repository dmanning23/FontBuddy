using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FontBuddyLib
{
	/// <summary>
	/// This dude draw some text with an outline.
	/// </summary>
	public class OutlineTextBuddy : FontBuddy
	{
		#region Members

		/// <summary>
		/// font buddy we are going to use to draw the outline
		/// </summary>
		private readonly FontBuddy _outlineWriter = new FontBuddy();

		/// <summary>
		/// color to draw the outline
		/// </summary>
		public Color OutlineColor { get; set; }

		/// <summary>
		/// The font this dude is "helping" with... overridden here so the shadow text uses the same font
		/// </summary>
		public override SpriteFont Font
		{
			get { return base.Font; }
			set
			{
				_outlineWriter.Font = value;
				base.Font = value;
			}
		}

		/// <summary>
		/// offset of our text to draw the shadow
		/// defaults to 0.0f, 3.0f
		/// </summary>
		public Vector2 ShadowOffset { get; set; }

		/// <summary>
		/// How much bigger than our text to make the outline... this is a point size, NOT A MUTLIPLIER
		/// </summary>
		public int OutlineSize { get; set; }

		#endregion //Members

		#region Properties

		protected FontBuddy OutlineWriter
		{
			get { return _outlineWriter; }
		}

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Constructor!
		/// </summary>
		public OutlineTextBuddy()
		{
			OutlineColor = Color.Black;
			ShadowOffset = new Vector2(0.0f, 3.0f);
			OutlineSize = 5;
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
		public override float Write(string strText, Vector2 position, Justify eJustification, float fScale, Color myColor, SpriteBatch mySpriteBatch, double dTime = 0.0f)
		{
			//draw the outline
			for (int x = -OutlineSize; x <= OutlineSize; x += OutlineSize / 2)
			{
				for (int y = -OutlineSize; y <= OutlineSize; y += OutlineSize / 2)
				{
					if (x == 0 && y == 0)
					{ 
						continue; 
					}

					Vector2 outlinePos = new Vector2(position.X + x, position.Y + y);
					OutlineWriter.Write(strText,
								outlinePos,
								eJustification,
								fScale,
								OutlineColor,
								mySpriteBatch,
								dTime);
				}
			}

			//draw my text
			return base.Write(strText, position, eJustification, fScale, myColor, mySpriteBatch, dTime);
		}

		#endregion //Methods
	}
}