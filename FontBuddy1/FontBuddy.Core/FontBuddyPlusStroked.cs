using FontStashSharp;
using Microsoft.Xna.Framework.Graphics;

namespace FontBuddyLib
{
	public class FontBuddyPlusStroked : FontBuddyPlus
	{
		public int OutlineSize { get; private set; }

		public FontBuddyPlusStroked(int outlineSize = 1)
		{
			OutlineSize = outlineSize;
		}

		protected override FontSystem CreateFontSystem(GraphicsDevice device)
		{
			var settings = new FontSystemSettings
			{
				Effect = FontSystemEffect.Stroked,
				EffectAmount = OutlineSize
			};
			settings.Effect = FontSystemEffect.Stroked;
			return new FontSystem(settings);
		}
	}
}