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

		private float m_fSelectionFade;

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
			m_fSelectionFade = 0.0f;
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
				m_fSelectionFade = (float)Math.Min(m_fSelectionFade + fadeSpeed, 1);
			}
			else
			{
				m_fSelectionFade = (float)Math.Max(m_fSelectionFade - fadeSpeed, 0);
			}

			//Pulsate the size of the text
			float pulsate = PulsateSize * (float)(Math.Sin(currentTime));
			float pulseScale = 1 + pulsate * 0.15f * m_fSelectionFade;

			//adjust the y position so it pulsates straight out
			Vector2 adjust = ((Font.MeasureString(text) * scale * pulseScale) - (Font.MeasureString(text) * scale)) / 2.0f;
			position.X -= adjust.X;

			WriteShadow(text, position, justification, scale, spriteBatch, time);

			//Draw the menu item, with the pulsing
			return DrawText(text, position, justification, scale, color, spriteBatch, time);
		}
	}
}