using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FontBuddyLib
{
	/// <summary>
	/// Defines the contract for font renderers that support shadow effects.
	/// </summary>
	public interface IShadowTextBuddy
	{
		/// <summary>
		/// Gets or sets the color used to draw the text shadow.
		/// </summary>
		/// <value>The shadow color. Default is typically <see cref="Color.Black"/>.</value>
		Color ShadowColor { get; set; }

		/// <summary>
		/// Gets or sets the pixel offset of the shadow from the main text position.
		/// </summary>
		/// <value>The shadow offset as a <see cref="Vector2"/>. Default is (0, 3).</value>
		Vector2 ShadowOffset { get; set; }

		/// <summary>
		/// Gets or sets the scale multiplier for the shadow relative to the main text.
		/// This is a multiplier of the font scale, not a point size.
		/// </summary>
		/// <value>The shadow size multiplier. Default is 1.05.</value>
		float ShadowSize { get; set; }
	}
}
