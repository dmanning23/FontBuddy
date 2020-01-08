using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FontBuddyLib
{
	public class ShakyTextBuddy : BaseFontBuddy
	{
		#region Properties

		/// <summary>
		/// How much the text shakes, higher values = more shake
		/// Defaults to 10
		/// </summary>
		/// <value>The shake amount.</value>
		public float ShakeAmount { get; set; }

		/// <summary>
		/// How fast to shake the text
		/// defaults to 10
		/// </summary>
		/// <value>The shake speed.</value>
		public float ShakeSpeed { get; set; }

		private FontStringCache Text { get; set; }

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="ShakyTextBuddy"/> class.
		/// Draw some text that shakes up and down!
		/// </summary>
		public ShakyTextBuddy()
		{
			ShakeAmount = 10.0f;
			ShakeSpeed = 10.0f;
			Text = new FontStringCache();
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

			var fKerning = Font.Spacing * scale;

			//update the string cache
			Text.UpdateText(text);

			//Get the correct location
			var textSize = Font.MeasureString(text) * scale;

			switch (justification)
			{
				case Justify.Right:
					{
						//move teh x value
						for (var i = 0; i < Text.StringCache.Count; i++)
						{
							//get teh size of the character
							textSize = Font.MeasureString(Text.StringCache[i]) * scale;
							position.X -= textSize.X;

							//get the kerning too
							position.X -= fKerning;
						}
					}
					break;

				case Justify.Center:
					{
						//move teh x value
						for (var i = 0; i < Text.StringCache.Count; i++)
						{
							//get teh size of the character
							textSize = Font.MeasureString(Text.StringCache[i]) * scale;
							position.X -= (textSize.X / 2.0f);

							//get the kerning too
							position.X -= fKerning / 2.0f;
						}
					}
					break;
			}

			//ok, draw each individual letter
			for (int i = 0; i < Text.StringCache.Count; i++)
			{
				var pulsate = MathHelper.Clamp((ShakeAmount * (float)(Math.Sin(time.CurrentTime * ShakeSpeed))), -ShakeAmount, ShakeAmount);
				if ((i % 2) == 0)
				{
					pulsate *= -1.0f;
				}
				var pulsingPosition = position;
				pulsingPosition.Y += pulsate;

				var subString = Text.StringCache[i];

				//Clamp (because we dont want pure black and white)
				Font.DrawString(subString, pulsingPosition, scale, color, spriteBatch);

				position.X += Font.MeasureString(subString).X * scale;
				position.X += fKerning;
			}

			//return the end of that string
			return position.X + textSize.X;
		}

		#endregion //Methods
	}
}