//using System.Diagnostics;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;

//namespace FontBuddyLib
//{
//	/// <summary>
//	/// This is an object for writing numbers.
//	/// When the number is changed, it will count up and countdown to the new value.
//	/// </summary>
//	public class NumberBuddy : IFontBuddy
//	{
//		#region Properties

//		#endregion //Properties

//		#region Methods

//		#endregion //Methods

//		/// <summary>
//		/// The font this dude is "helping" with
//		/// </summary>
//		SpriteFont Font { set; }

//		/// <summary>
//		/// given a content manager and a resource name, load the resource as a bitmap font
//		/// </summary>
//		/// <param name="rContent"></param>
//		/// <param name="strResource"></param>
//		void LoadContent(ContentManager rContent, string strResource);

//		/// <summary>
//		/// write something on the screen
//		/// </summary>
//		/// <param name="text">the text to write on the screen</param>
//		/// <param name="position">where to write at... either upper left, upper center, or upper right, depending on justication</param>
//		/// <param name="justification">how to justify the text</param>
//		/// <param name="scale">how big to write.  This is not a point size to draw at, it is a multiple of the default font size!</param>
//		/// <param name="color">the color to draw the text</param>
//		/// <param name="spriteBatch">spritebatch to use to render the text</param>
//		/// <param name="dTime">Most of the other font buddy classes use time somehow, but can jsut send in 0.0f for this dude or ignoer it</param>
//		float Write(string text,
//			Vector2 position,
//			Justify justification,
//			float scale,
//			Color color,
//			SpriteBatch spriteBatch,
//			double dTime = 0.0f);

//		/// <summary>
//		/// write something on the screen
//		/// </summary>
//		/// <param name="text">the text to write on the screen</param>
//		/// <param name="Position">where to write at... either upper left, upper center, or upper right, depending on justication</param>
//		/// <param name="justification">how to justify the text</param>
//		/// <param name="scale">how big to write.  This is not a point size to draw at, it is a multiple of the default font size!</param>
//		/// <param name="color">the color to draw the text</param>
//		/// <param name="spriteBatch">spritebatch to use to render the text</param>
//		/// <param name="dTime">Most of the other font buddy classes use time somehow, but can jsut send in 0.0f for this dude or ignoer it</param>
//		float Write(string text,
//			Point Position,
//			Justify justification,
//			float scale,
//			Color color,
//			SpriteBatch spriteBatch,
//			double dTime = 0.0f);
//	}
//}