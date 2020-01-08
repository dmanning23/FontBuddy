using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteFontPlus;
using System;
using System.Collections.Generic;

namespace FontBuddyLib
{
	public class FontBuddyPlus : IFontBuddy
	{
		#region Properties

		private DynamicSpriteFont DynamicSpriteFont { get; set; }

		public SpriteFont Font { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public SpriteEffects SpriteEffects { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public float Rotation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public float Spacing => DynamicSpriteFont.Spacing;

		#endregion //Properties

		#region Methods

		public FontBuddyPlus()
		{
		}

		public void LoadContent(ContentManager content, string resourceName, bool useFontBuddyPlus = true, int fontSize = 24)
		{
			if (!useFontBuddyPlus)
			{
				throw new Exception("FontBuddyPlus.LoadContent was passed useFontBuddyPlus = false");
			}

			if (null == DynamicSpriteFont)
			{
				var resource = content.Load<byte[]>(resourceName);
				DynamicSpriteFont = DynamicSpriteFont.FromTtf(resource, fontSize, 512, 512);
				DynamicSpriteFont.Size = fontSize;
				DynamicSpriteFont.CurrentAtlasFull += DynamicSpriteFont_CurrentAtlasFull;
			}
		}

		private void DynamicSpriteFont_CurrentAtlasFull(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		public Vector2 MeasureString(string text)
		{
			return DynamicSpriteFont.MeasureString(text);
		}

		public float Write(string text, Vector2 position, Justify justification, float scale, Color color, SpriteBatch spriteBatch, GameClock time)
		{
			//if this thing is empty, dont do anything
			if (string.IsNullOrEmpty(text))
			{
				return position.X;
			}

			position = LineFormatter.JustifiedPosition(text, position, justification, scale, this);
			DrawString(text, position, scale, color, spriteBatch);

			//return the end of that string
			return position.X + (MeasureString(text).X * scale);
		}

		public void DrawString(string text, Vector2 position, float scale, Color color, SpriteBatch spriteBatch)
		{
			//okay, draw the actual string
			spriteBatch.DrawString(DynamicSpriteFont, text, position, color, new Vector2(scale));
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
