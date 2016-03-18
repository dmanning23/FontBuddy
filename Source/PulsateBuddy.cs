using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FontBuddyLib
{
	/// <summary>
	/// This dude writes the text in big pulsating letters, with the outline below it.
	/// </summary>
	public class PulsateBuddy : ShadowTextBuddy
	{
		#region Members

		/// <summary>
		/// how big the pulsate the text
		/// </summary>
		public float PulsateSize { get; set; }

		/// <summary>
		/// How fast to pulsate the speed
		/// </summary>
		public float PulsateSpeed { get; set; }

		#endregion //Members

		/// <summary>
		/// Constructor
		/// </summary>
		public PulsateBuddy()
		{
			PulsateSize = 1.0f;
			PulsateSpeed = 4.0f;
		}

		public override float Write(string text,
			Vector2 position,
			Justify justification,
			float scale,
			Color color,
			SpriteBatch spriteBatch,
			GameClock time)
		{
			//First draw the shadow
			ShadowWriter.Write(text,
							   position,
							   justification,
							   ShadowSize * scale,
							   ShadowColor,
							   spriteBatch,
							   time);

			//multiply the time by the speed
			float currentTime = time.CurrentTime;
			currentTime *= PulsateSpeed;

			//Pulsate the size of the text
			float pulsate = PulsateSize * (float)(Math.Sin(currentTime) + 1.0f);
			float pulseScale = 1 + pulsate * 0.15f;

			//adjust the y position so it pulsates straight out
			Vector2 adjust = ((Font.MeasureString(text) * scale * pulseScale) - (Font.MeasureString(text) * scale)) / 2.0f;
			position.Y -= adjust.Y;

			//Draw the menu item, with the pulsing
			return DrawText(text, position, justification, scale * pulseScale, color, spriteBatch, time);
		}
	}
}