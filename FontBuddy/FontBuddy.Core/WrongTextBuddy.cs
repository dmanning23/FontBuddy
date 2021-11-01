using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FontBuddyLib
{
	/// <summary>
	/// This dude writes the text in big pulsating letters, with the outline below it.
	/// </summary>
	public class WrongTextBuddy : ShadowTextBuddy
	{
		#region Fields

		private float _pulsateSpeed;

		private GameClock _timer;

		#endregion //Fields

		#region Members

		private float SelectionFade { get; set; }

		/// <summary>
		/// how big the pulsate the text
		/// </summary>
		public float PulsateSize { get; set; }

		/// <summary>
		/// How fast to pulsate the speed. Default value is 25f
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
		/// If the pulsate is turned on/off, it will ease into the pulsating
		/// </summary>
		public bool Selected { get; set; }

		#endregion //Members

		/// <summary>
		/// Constructor
		/// </summary>
		public WrongTextBuddy()
		{
			PulsateSize = 1.0f;
			PulsateSpeed = 25.0f;
			Selected = true;
			SelectionFade = 0.0f;
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
			_timer.Update(time);

			if (string.IsNullOrEmpty(text))
			{
				return position.X;
			}

			float currentTime = _timer.CurrentTime;

			// When the menu selection changes, entries gradually fade between
			// their selected and deselected appearance, rather than instantly
			// popping to the new state.
			double fadeSpeed = currentTime * 4.0f;

			if (Selected)
			{
				SelectionFade = (float)Math.Min(SelectionFade + fadeSpeed, 1);
			}
			else
			{
				SelectionFade = (float)Math.Max(SelectionFade - fadeSpeed, 0);
			}

			//Pulsate the size of the text
			var pulsate = PulsateSize * (float)(Math.Sin(currentTime));
			var pulseScale = 1 + pulsate * 0.15f * SelectionFade;

			//adjust the y position so it pulsates straight out
			var adjust = ((Font.MeasureString(text) * scale * pulseScale) - (Font.MeasureString(text) * scale)) / 2.0f;
			position.X -= adjust.X;

			WriteShadow(text, position, justification, scale, spriteBatch, time);

			//Draw the menu item, with the pulsing
			return Font.Write(text, position, justification, scale, color, spriteBatch, time);
		}
	}
}