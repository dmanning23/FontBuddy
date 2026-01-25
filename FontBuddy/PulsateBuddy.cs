using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FontBuddyLib
{
	/// <summary>
	/// A text renderer that creates a pulsating size effect with a shadow behind it.
	/// The text smoothly grows and shrinks over time while the shadow remains static.
	/// </summary>
	/// <remarks>
	/// The pulsation effect uses a sine wave to smoothly scale the text up and down.
	/// The shadow is drawn at a constant size behind the pulsating text.
	/// When <see cref="StraightPulsate"/> is true, the text expands from its center horizontally.
	/// </remarks>
	public class PulsateBuddy : ShadowTextBuddy
	{
		#region Fields

		private float _pulsateSpeed;

		private GameClock _timer;

		#endregion //Fields

		#region Members

		/// <summary>
		/// Gets or sets the amplitude of the pulsation effect.
		/// Higher values result in more dramatic size changes.
		/// </summary>
		/// <value>The pulsation amplitude. Default is 1.0.</value>
		public float PulsateSize { get; set; }

		/// <summary>
		/// Gets or sets the speed of the pulsation animation.
		/// Higher values result in faster pulsing.
		/// </summary>
		/// <value>The pulsation speed. Default is 4.0.</value>
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
		/// Gets or sets whether the pulsation expands from the center horizontally.
		/// When true, the text position is adjusted to maintain center alignment during scaling.
		/// </summary>
		/// <value><c>true</c> to expand from center; otherwise, <c>false</c>. Default is <c>true</c>.</value>
		public bool StraightPulsate
		{
			get; set;
		}

		#endregion //Members

		/// <summary>
		/// Initializes a new instance of the <see cref="PulsateBuddy"/> class with default settings.
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

		/// <summary>
		/// Renders text with a pulsating size effect and shadow.
		/// </summary>
		/// <param name="text">The text to render.</param>
		/// <param name="position">The position to render at. Interpretation depends on justification.</param>
		/// <param name="justification">The text justification mode.</param>
		/// <param name="scale">The base scale multiplier for the font size.</param>
		/// <param name="color">The color to render the text in.</param>
		/// <param name="spriteBatch">The SpriteBatch to use for rendering.</param>
		/// <param name="time">The game clock for the animation timing.</param>
		/// <returns>The X position at the end of the rendered text.</returns>
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