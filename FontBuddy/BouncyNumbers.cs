using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text;

namespace FontBuddyLib
{
	/// <summary>
	/// A text renderer that displays numbers with an animated count-up effect followed by a scale pop.
	/// The number counts from a start value to a target value, then scales up dramatically before settling.
	/// </summary>
	/// <remarks>
	/// The animation consists of three phases:
	/// 1. Count-up phase: The number rapidly counts from start to target
	/// 2. Scale phase: The number pops to a larger size then shrinks back
	/// 3. Display phase: The final number is displayed at a slightly larger scale until IsDead becomes true
	/// </remarks>
	public class BouncyNumbers : OutlineTextBuddy
	{
		#region Properties

		/// <summary>
		/// Gets or sets the starting number for the count animation.
		/// </summary>
		private int StartNumber { get; set; }

		/// <summary>
		/// Gets or sets the target number that the animation counts towards.
		/// </summary>
		private int TargetNumber { get; set; }

		/// <summary>
		/// Gets or sets the difference between target and start numbers.
		/// </summary>
		private int Delta { get; set; }

		/// <summary>
		/// Gets or sets the timer used to control the animation phases.
		/// </summary>
		private CountdownTimer Timer { get; set; }

		/// <summary>
		/// Gets or sets the scale multiplier applied during the pop phase.
		/// The number will momentarily scale to this size before shrinking.
		/// </summary>
		/// <value>The peak scale during the pop. Default is 2.5.</value>
		public float ScaleAtEnd { get; set; }

		/// <summary>
		/// Gets or sets the duration of the count-up phase in seconds.
		/// This is automatically calculated based on the delta.
		/// </summary>
		private float CountUpTime { get; set; }

		/// <summary>
		/// Gets or sets the minimum pause duration before the scale pop begins.
		/// </summary>
		/// <value>The minimum pause in seconds. Default is 1.0.</value>
		public float ScalePause { get; set; }

		/// <summary>
		/// Gets or sets the duration of the scale pop phase in seconds.
		/// </summary>
		/// <value>The scale animation duration. Default is 1.0.</value>
		public float ScaleTime { get; set; }

		/// <summary>
		/// Gets or sets how long the number remains visible after the scale animation completes.
		/// </summary>
		/// <value>The display duration after scaling. Default is 3.0.</value>
		public float KillTime { get; set; }

		/// <summary>
		/// Gets or sets the final scale multiplier after the pop animation settles.
		/// The number will be displayed at this scale until IsDead becomes true.
		/// </summary>
		/// <value>The final resting scale. Default is 1.2.</value>
		public float Rescale { get; set; }

		/// <summary>
		/// Gets whether the animation has completed and the number should no longer be displayed by this renderer.
		/// </summary>
		/// <value><c>true</c> if the animation is complete; otherwise, <c>false</c>.</value>
		public bool IsDead
		{
			get
			{
				//paused timer or at the end of all the time
				return (KillTime > 0f && !Timer.HasTimeRemaining);
			}
		}

		/// <summary>
		/// Gets or sets whether the scale animation expands from the center horizontally.
		/// </summary>
		/// <value><c>true</c> to expand from center; otherwise, <c>false</c>. Default is <c>true</c>.</value>
		public bool StraightPulsate
		{
			get; set;
		}

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="BouncyNumbers"/> class with default settings.
		/// </summary>
		public BouncyNumbers()
		{
			StraightPulsate = true;
			Timer = new CountdownTimer();
			Timer.Stop();
			ScaleTime = 1f;
			ScaleAtEnd = 2.5f;
			KillTime = 3f;
			Rescale = 1.2f;
			ScalePause = 1f;
		}

		/// <summary>
		/// Starts the count animation from a start number to a target number.
		/// </summary>
		/// <param name="startNumber">The number to start counting from.</param>
		/// <param name="targetNumber">The number to count to.</param>
		public void Start(int startNumber, int targetNumber)
		{
			StartNumber = startNumber;
			TargetNumber = targetNumber;
			Delta = TargetNumber - StartNumber;

			//get the num delta
			int delta = Math.Abs(Delta);

			//adjust the target number as necessary
			CountUpTime = Math.Min(5.0f, Math.Max(ScalePause, delta / 300f));
			Timer.Start(CountUpTime + ScaleTime + KillTime);
		}

		/// <summary>
		/// Renders the animated number with count-up and scale effects.
		/// </summary>
		/// <param name="text">The prefix text to render before the number.</param>
		/// <param name="position">The position to render at. Interpretation depends on justification.</param>
		/// <param name="justification">The text justification mode.</param>
		/// <param name="scale">The base scale multiplier for the font size.</param>
		/// <param name="color">The color to render the text in.</param>
		/// <param name="spriteBatch">The SpriteBatch to use for rendering.</param>
		/// <param name="time">The game clock for the animation timing.</param>
		/// <returns>The X position at the end of the rendered text, or the original X if the animation hasn't started.</returns>
		public override float Write(string text,
			Vector2 position,
			Justify justification,
			float scale,
			Color color,
			SpriteBatch spriteBatch,
			GameClock time)
		{
			var str = new StringBuilder();
			str.Append(text);

			//update the timer we are using
			Timer.Update(time);

			if (!IsDead)
			{
				//are we before or after the cutoff
				if (Timer.CurrentTime <= CountUpTime)
				{
					//lerp up to the desired number
					var currentNumber = StartNumber;
					currentNumber += (int)((Delta * Timer.CurrentTime) / CountUpTime);

					//add 1 so it doest start at 0
					currentNumber += ((Delta >= 0) ? 1 : -1);

					//write number
					str.Append(currentNumber);
					return base.Write(str.ToString(),
						position,
						justification,
						scale,
						color,
						spriteBatch,
						Timer);
				}
				else if (Timer.CurrentTime <= (CountUpTime + ScaleTime))
				{
					//add the target number
					str.Append(TargetNumber);

					//this is the amount we want to end the scale at
					scale *= Rescale;

					//current time - countuptime starts us at 0.0, which is good for lerping purposes
					var currentTime = Timer.CurrentTime - CountUpTime;

					//convert the amount of time to a number between 0.0 and 1.0
					var lerp = currentTime / ScaleTime;

					//lerp from the start scale to the end scale
					var finalScale = MathHelper.Lerp(ScaleAtEnd, scale, lerp);

					//adjust the position to draw based on how much we are scaling
					var textSize = Font.MeasureString(str.ToString());
					var adjust = ((textSize * finalScale) - (textSize * scale)) / 2f;
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

					return base.Write(str.ToString(),
						position,
						justification,
						finalScale,
						color,
						spriteBatch,
						Timer);
				}
				else
				{
					//make the text a little bigger
					scale *= Rescale;

					//write the final number
					str.Append(TargetNumber);
					return base.Write(str.ToString(),
						position,
						justification,
						scale,
						color,
						spriteBatch,
						Timer);
				}
			}

			//if this thing hasnt been started yet, return nothing!
			return position.X;
		}

		#endregion //Methods
	}
}