using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FontBuddyLib
{
	/// <summary>
	/// A text renderer designed for menu items that pulsates when selected.
	/// The pulsation effect fades in/out smoothly when the selection state changes.
	/// </summary>
	/// <remarks>
	/// This effect is ideal for menu systems where selected items should visually "pop".
	/// The pulsation only occurs when <see cref="Selected"/> is true, and smoothly
	/// transitions in and out rather than abruptly starting/stopping.
	/// </remarks>
	public class WrongTextBuddy : ShadowTextBuddy
	{
		#region Fields

		private float _pulsateSpeed;

		private GameClock _timer;

		#endregion //Fields

		#region Members

		/// <summary>
		/// Gets or sets the current fade value for the selection effect (0.0 to 1.0).
		/// </summary>
		private float SelectionFade { get; set; }

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
		/// <value>The pulsation speed. Default is 25.0.</value>
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
		/// Gets or sets whether this menu item is currently selected.
		/// When true, the text will pulsate; when false, the pulsation fades out.
		/// </summary>
		/// <value><c>true</c> if the item is selected; otherwise, <c>false</c>. Default is <c>true</c>.</value>
		public bool Selected { get; set; }

		#endregion //Members

		/// <summary>
		/// Initializes a new instance of the <see cref="WrongTextBuddy"/> class with default settings.
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

		/// <summary>
		/// Renders text with a selection-based pulsating effect and shadow.
		/// The pulsation smoothly fades in when selected and fades out when deselected.
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