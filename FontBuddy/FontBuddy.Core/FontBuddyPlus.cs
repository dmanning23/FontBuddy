using FontStashSharp;
using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FontBuddyLib
{
	public class FontBuddyPlus : IFontBuddy, IDisposable
	{
		#region Properties

		public FontSystem FontSystem { get; set; }
		public SpriteFontBase SpriteFont { get; set; }// = _fontSystem.GetFont(18);

		public SpriteFont Font { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public SpriteEffects SpriteEffects
		{
			get => SpriteEffects.None;
			set
			{
				if (value != SpriteEffects.None)
				{
					throw new NotImplementedException();
				}
			}
		}

		public float Rotation { get; set; }

		public float Spacing => FontSystem.CharacterSpacing;

		#endregion //Properties

		#region Methods

		public FontBuddyPlus()
		{
		}

		public void Dispose()
		{
			FontSystem?.Dispose();
			FontSystem = null;
		}

		public void LoadContent(ContentManager content, string resourceName, bool useFontBuddyPlus = true, int fontSize = 24)
		{
			if (!useFontBuddyPlus)
			{
				throw new Exception("FontBuddyPlus.LoadContent was passed useFontBuddyPlus = false");
			}

			if (null == FontSystem)
			{
				var graphicDevice = content.ServiceProvider.GetService(typeof(IGraphicsDeviceService)) as IGraphicsDeviceService;
				if (null == graphicDevice)
				{
					throw new ArgumentNullException(nameof(IGraphicsDeviceService));
				}

				FontSystem = CreateFontSystem(graphicDevice.GraphicsDevice);

				AddFont(content, resourceName, fontSize);
			}
		}

		public void AddFont(ContentManager content, string resourceName, int fontSize = 24)
		{
			var resource = content.Load<byte[]>(resourceName);
			FontSystem.AddFont(resource);
			SpriteFont = FontSystem.GetFont(fontSize);
			FontSystem.DefaultCharacter = '\uFFFD';
			FontSystem.CurrentAtlasFull += DynamicSpriteFont_CurrentAtlasFull;
		}

		protected virtual FontSystem CreateFontSystem(GraphicsDevice device)
		{
			return new FontSystem();
		}

		private void DynamicSpriteFont_CurrentAtlasFull(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		public Vector2 MeasureString(string text)
		{
			var width = SpriteFont.MeasureString(text).X;
			var height = SpriteFont.MeasureString("yY").Y; //make sure the string height includes both ascender and descender
			return new Vector2(width, height);
		}

		public float Write(string text, Vector2 position, Justify justification, float scale, Color color, SpriteBatch spriteBatch, GameClock time)
		{
			//if this thing is empty, dont do anything
			if (string.IsNullOrEmpty(text))
			{
				return position.X;
			}

			position = LineFormatter.JustifiedPosition(text, position, this, justification, scale);
			DrawString(text, position, scale, color, spriteBatch);

			//return the end of that string
			return position.X + (MeasureString(text).X * scale);
		}

		public void DrawString(string text, Vector2 position, float scale, Color color, SpriteBatch spriteBatch)
		{
			//okay, draw the actual string
			spriteBatch.DrawString(SpriteFont, text, position, color, new Vector2(scale), Rotation, Vector2.Zero, 0);
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