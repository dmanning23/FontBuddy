using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FontBuddyLib
{
	public interface IShadowTextBuddy
	{
		/// <summary>
		/// color to draw the shadow
		/// </summary>
		Color ShadowColor { get; set; }

		/// <summary>
		/// offset of our text to draw the shadow
		/// defaults to 0.0f, 3.0f
		/// </summary>
		Vector2 ShadowOffset { get; set; }

		/// <summary>
		/// How much bigger than our text to make the shadow... this is a multiplier of the font size, NOT A POINT SIZE!!!
		/// defaults to 1.05f
		/// </summary>
		float ShadowSize { get; set; }
	}
}
