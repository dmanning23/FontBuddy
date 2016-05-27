using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text;

namespace FontBuddyLib
{
	/// <summary>
	/// This takes a number and counts up to it for one second
	/// then gets big at the end
	/// </summary>
	public class BouncyNumbers : OutlineTextBuddy
	{
		#region Properties

		/// <summary>
		/// The number we are starting to count at
		/// </summary>
		private int StartNumber { get; set; }

		/// <summary>
		/// The number we are counting up to
		/// </summary>
		private int TargetNumber { get; set; }

		/// <summary>
		/// the range between start and target
		/// </summary>
		private int Delta { get; set; }

		/// <summary>
		/// thing used to count up from 0 -> target number
		/// call start on this dude when you want to display
		/// </summary>
		private CountdownTimer Timer { get; set; }

		/// <summary>
		/// After the thing is done counting up, how much to scale it.
		/// Defaults ot 1.5f
		/// </summary>
		public float ScaleAtEnd { get; set; }

		/// <summary>
		/// how long to count up from 0
		/// defaults to 1.0
		/// </summary>
		public float CountUpTime { get; set; }

		/// <summary>
		/// how long should scale from countup time to normal size
		/// defaulst to 1.0f
		/// </summary>
		public float ScaleTime { get; set; }

		/// <summary>
		/// How long to display this number after the scale time runs out
		/// </summary>
		public float KillTime { get; set; }

		/// <summary>
		/// We want this number to be a little bit bigger after the scale runs out...
		/// deafulst to 1.2f
		/// </summary>
		public float Rescale { get; set; }

		/// <summary>
		/// once the text hits CountUpTime + ScaleTime + ScaleTime it is dead
		/// </summary>
		public bool IsDead
		{
			get
			{
				//paused timer or at the end of all the time
				return (KillTime > 0f && !Timer.HasTimeRemaining());
			}
		}

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Cosntructor!
		/// </summary>
		public BouncyNumbers()
		{
			Timer = new CountdownTimer();
			Timer.Stop();
			ScaleTime = 1f;
			ScaleAtEnd = 2.5f;
			KillTime = 3f;
			Rescale = 1.2f;
		}

		public void Start(int startNumber, int targetNumber)
		{
			StartNumber = startNumber;
			TargetNumber = targetNumber;
			Delta = TargetNumber - StartNumber;

			//get the num delta
			int delta = Math.Abs(Delta);

			//adjust the target number as necessary
			CountUpTime = Math.Min(5.0f, Math.Max(1f, delta / 300f));
			Timer.Start(CountUpTime + ScaleTime + KillTime);
		}

		public override float Write(string text,
			Vector2 position,
			Justify justification,
			float scale,
			Color color,
			SpriteBatch spriteBatch,
			GameClock time)
		{
			StringBuilder str = new StringBuilder();
			str.Append(text);

			//update the timer we are using
			Timer.Update(time);

			if (!IsDead)
			{
				//are we before or after the cutoff
				if (Timer.CurrentTime <= CountUpTime)
				{
					//lerp up to the desired number
					int currentNumber = StartNumber;
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
					float currentTime = Timer.CurrentTime - CountUpTime;

					//convert the amount of time to a number between 0.0 and 1.0
					float lerp = currentTime / ScaleTime;

					//lerp from the start scale to the end scale
					float finalScale = MathHelper.Lerp(ScaleAtEnd, scale, lerp);

					//adjust the position to draw based on how much we are scaling
					var textSize = Font.MeasureString(str.ToString());
					Vector2 adjust = ((textSize * finalScale) - (textSize * scale));
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