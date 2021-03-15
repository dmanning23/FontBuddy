using FontStashSharp;
using Microsoft.Xna.Framework.Graphics;

namespace FontBuddyLib
{
	public class FontBuddyPlusStroked : FontBuddyPlus
	{
		protected override FontSystem CreateFontSystem(GraphicsDevice device)
		{
			return FontSystemFactory.CreateStroked(device, 1);
		}
	}
}
