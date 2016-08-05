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
		#region Fields

		private float _pulsateSpeed;

		private GameClock _timer;

		#endregion //Fields

		#region Members

		/// <summary>
		/// how big the pulsate the text. Default value is 1f
		/// </summary>
		public float PulsateSize { get; set; }

		/// <summary>
		/// How fast to pulsate the speed. Default value is 4f
		/// </summary>
		public float PulsateSpeed
		{
			get
			{
				return _pulsateSpeed;
			}
			set
			{
				_pulsateSpeed = value;
				if (null != _timer)
				{
					_timer.TimerSpeed = _pulsateSpeed;
				}
			}
		}

		/// <summary>
		/// How much to scale the size of the pulsate. Default is 1f
		/// </summary>
		public float PulsateScale { get; set; }

		#endregion //Members

		/// <summary>
		/// Constructor
		/// </summary>
		public PulsateBuddy()
		{
			PulsateSize = 1.0f;
			PulsateSpeed = 4.0f;
			PulsateScale = 1f;
			_timer = new GameClock()
			{
				TimerSpeed = PulsateSpeed
			};
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

			_timer.Update(time);
			float currentTime = _timer.CurrentTime;

			//Pulsate the size of the text
			float pulsate = PulsateSize * (float)(Math.Sin(currentTime - (Math.PI * 0.5)) + 1.0f);
			float pulseScale = PulsateScale + pulsate * 0.15f;

			//adjust the y position so it pulsates straight out
			Vector2 adjust = ((Font.MeasureString(text) * scale * pulseScale) - (Font.MeasureString(text) * scale)) / 2.0f;
			position.Y -= adjust.Y;

			//Draw the menu item, with the pulsing
			return DrawText(text, position, justification, scale * pulseScale, color, spriteBatch, time);
		}
	}
}