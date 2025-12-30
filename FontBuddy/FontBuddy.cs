using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

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
		protected virtual SpriteFont Font { get; set; }

		public virtual SpriteEffects SpriteEffects { get; set; }

		public virtual float Rotation { get; set; }

		public float Spacing => Font.Spacing;

		#endregion //Properties

		#region Methods

		/// <summary>
		/// constructor... you need to set the font or call load content if you use this one
		/// </summary>
		public FontBuddy()
		{
			SpriteEffects = SpriteEffects.None;
		}

		public void Dispose()
		{
		}

		/// <summary>
		/// Get the size of a specific peice of text.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public Vector2 MeasureString(string text)
		{
			return Font.MeasureString(text);
        }

		/// <summary>
		/// given a content manager and a resource name, load the resource as a bitmap font
		/// </summary>
		/// <param name="content"></param>
		/// <param name="resource"></param>
		public void LoadContent(ContentManager content, string resource, bool useFontBuddyPlus = false, int fontSize = 0)
		{
			if (useFontBuddyPlus)
			{
				throw new Exception("FontBuddy.LoadContent was passed useFontBuddyPlus = true");
			}

			//load font
			if (null == Font)
			{
				Font = content.Load<SpriteFont>(resource);
			}
		}

		public virtual float Write(string text,
			Vector2 position,
			Justify justification,
			float scale,
			Color color,
			SpriteBatch spriteBatch,
			GameClock time)
		{
			return DrawText(text, position, justification, scale, color, spriteBatch, time);
		}

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
		protected float DrawText(string text,
			Vector2 position,
			Justify justification,
			float scale,
			Color color,
			SpriteBatch spriteBatch,
			GameClock time)
		{
			//if this thing is empty, dont do anything
			if (string.IsNullOrEmpty(text))
			{
				return position.X;
			}

			position = LineFormatter.JustifiedPosition(text, position, this, justification, scale);

			//okay, draw the actual string
			DrawString(text, position, scale, color, spriteBatch);

			//return the end of that string
			return position.X + (MeasureString(text).X * scale);
		}

		public void DrawString(string text, Vector2 position, float scale, Color color, SpriteBatch spriteBatch)
		{
			spriteBatch.DrawString(Font,
									text,
									position,
									color,
									Rotation,
									Vector2.Zero,
									scale,
									SpriteEffects,
									0);
		}

		public List<string> BreakTextIntoList(string text, int rowWidth)
		{
			return LineFormatter.BreakTextIntoList(text, rowWidth, this);
		}

		public float ScaleToFit(string text, int rowWidth)
		{
			return LineFormatter.ScaleToFit(text, rowWidth, this);
		}

		public float ShrinkToFit(string text, int rowWidth)
		{
			return LineFormatter.ShrinkToFit(text, rowWidth, this);
		}

		public bool NeedsToShrink(string text, float scale, int rowWidth)
		{
			return LineFormatter.NeedsToShrink(text, scale, rowWidth, this);
		}

		#endregion //Methods
	}
}