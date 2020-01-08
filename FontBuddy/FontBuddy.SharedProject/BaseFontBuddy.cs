using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FontBuddyLib
{
	public abstract class BaseFontBuddy : IFontBuddy
	{
		/// <summary>
		/// font buddy we are going to use to draw the outline
		/// </summary>
		public IFontBuddy Font { get; set; }

		public float Spacing => Font.Spacing;

		private SpriteEffects _spriteEffects;
		public SpriteEffects SpriteEffects
		{
			get
			{
				return _spriteEffects;
			}
			set
			{
				_spriteEffects = value;
				if (null != Font)
				{
					Font.SpriteEffects = SpriteEffects;
				}
			}
		}

		private float _rotation;
		public float Rotation
		{
			get
			{
				return _rotation;
			}
			set
			{
				_rotation = value;
				if (null != Font)
				{
					Font.Rotation = Rotation;
				}
			}
		}

		public void LoadContent(ContentManager content, string resourceName, bool useFontBuddyPlus = false, int fontSize = 24)
		{
			if (useFontBuddyPlus)
			{
				Font = new FontBuddyPlus();
			}
			else
			{
				Font = new FontBuddy();
				Font.SpriteEffects = SpriteEffects;
				Font.Rotation = Rotation;
			}

			Font.LoadContent(content, resourceName, useFontBuddyPlus, fontSize);
		}

		public Vector2 MeasureString(string text)
		{
			return Font.MeasureString(text);
		}

		public List<string> BreakTextIntoList(string text, int rowWidth)
		{
			return Font.BreakTextIntoList(text, rowWidth);
		}

		public float ScaleToFit(string text, int rowWidth)
		{
			return Font.ScaleToFit(text, rowWidth);
		}

		public float ShrinkToFit(string text, int rowWidth)
		{
			return Font.ShrinkToFit(text, rowWidth);
		}

		public bool NeedsToShrink(string text, float scale, int rowWidth)
		{
			return Font.NeedsToShrink(text, scale, rowWidth);
		}

		public abstract float Write(string text, Vector2 position, Justify justification, float scale, Color color, SpriteBatch spriteBatch, GameClock time);

		public void DrawString(string text, Vector2 position, float scale, Color color, SpriteBatch spriteBatch)
		{
			Font.DrawString(text, position, scale, color, spriteBatch);
		}
	}
}
