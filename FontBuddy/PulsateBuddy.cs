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

		public bool StraightPulsate
		{
			get; set;
		}

		#endregion //Members

		/// <summary>
		/// Constructor
		/// </summary>
		public PulsateBuddy() : base()
		{
			StraightPulsate = true;
			PulsateSize = 1.0f;
			PulsateSpeed = 4.0f;
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
			if (string.IsNullOrEmpty(text))
			{
				return position.X;
			}

			//First draw the shadow
			Font.Write(text,
							   position,
							   justification,
							   ShadowSize * scale,
							   ShadowColor,
							   spriteBatch,
							   time);

			_timer.Update(time);
			var currentTime = _timer.CurrentTime;

			//Pulsate the size of the text
			var pulsate = PulsateSize * (float)(Math.Sin(currentTime - (Math.PI * 0.5)) + 1.0f);
			pulsate *= 0.15f; //make it waaay smaller
			pulsate += 1; //bump it up so it starts at 1

			//adjust the y position so it pulsates straight out
			var textSize = Font.MeasureString(text.ToString());
			var adjust = ((textSize * scale * pulsate) - (textSize * scale)) / 2f;
			if (StraightPulsate)
			{
				switch (justification)
				{
					case Justify.Left:
						{
							position.X -= adjust.X;
						}
						break;
					case Justify.Right:
						{
							position.X += adjust.X;
						}
						break;
				};
			}
			position.Y -= adjust.Y;

			//Draw the menu item, with the pulsing
			return Font.Write(text, position, justification, scale * pulsate, color, spriteBatch, time);
		}
	}
}