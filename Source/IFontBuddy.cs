using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FontBuddyLib
{
	/// <summary>
	/// An interface for the font buddies.
	/// </summary>
	public interface IFontBuddy
	{
		/// <summary>
		/// The font this dude is "helping" with
		/// </summary>
		SpriteFont Font { get; set; }

		/// <summary>
		/// given a content manager and a resource name, load the resource as a bitmap font
		/// </summary>
		/// <param name="rContent"></param>
		/// <param name="strResource"></param>
		void LoadContent(ContentManager rContent, string strResource);

		/// <summary>
		/// write something on the screen
		/// </summary>
		/// <param name="text">the text to write on the screen</param>
		/// <param name="position">where to write at... either upper left, upper center, or upper right, depending on justication</param>
		/// <param name="justification">how to justify the text</param>
		/// <param name="scale">how big to write.  This is not a point size to draw at, it is a multiple of the default font size!</param>
		/// <param name="color">the color to draw the text</param>
		/// <param name="spriteBatch">spritebatch to use to render the text</param>
		/// <param name="time">Most of the other font buddy classes use time somehow, but can jsut send in 0.0f for this dude or ignoer it</param>
		float Write(string text,
			Vector2 position,
			Justify justification,
			float scale,
			Color color,
			SpriteBatch spriteBatch,
			GameClock time);

		/// <summary>
		/// write something on the screen
		/// </summary>
		/// <param name="text">the text to write on the screen</param>
		/// <param name="position">where to write at... either upper left, upper center, or upper right, depending on justication</param>
		/// <param name="justification">how to justify the text</param>
		/// <param name="scale">how big to write.  This is not a point size to draw at, it is a multiple of the default font size!</param>
		/// <param name="color">the color to draw the text</param>
		/// <param name="spriteBatch">spritebatch to use to render the text</param>
		/// <param name="time">Most of the other font buddy classes use time somehow, but can jsut send in 0.0f for this dude or ignoer it</param>
		float Write(string text,
			Point position,
			Justify justification,
			float scale,
			Color color,
			SpriteBatch spriteBatch,
			GameClock time);
	}
}