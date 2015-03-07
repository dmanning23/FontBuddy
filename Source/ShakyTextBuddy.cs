using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FontBuddyLib
{
	public class ShakyTextBuddy : FontBuddy
	{
		#region Fields

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

		#endregion //Fields

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="ShakyTextBuddy"/> class.
		/// Draw some text that shakes up and down!
		/// </summary>
		public ShakyTextBuddy()
		{
			ShakeAmount = 10.0f;
			ShakeSpeed = 10.0f;
		}

		public override float Write(string text,
			Vector2 position,
			Justify justification,
			float scale,
			Color color,
			SpriteBatch spriteBatch,
			GameClock time)
		{
			float fKerning = Font.Spacing * scale;

			//Get the correct location
			Vector2 textSize = Font.MeasureString(text) * scale;
			switch (justification)
			{
				case Justify.Right:
				{
					//move teh x value
					for (int i = 0; i < text.Length; i++)
					{
						//get teh size of the character
						string strSubString = "" + text[i];
						textSize = Font.MeasureString(strSubString) * scale;
						position.X -= textSize.X;

						//get the kerning too
						position.X -= fKerning;
					}
				}
				break;

				case Justify.Center:
				{
					//move teh x value
					for (int i = 0; i < text.Length; i++)
					{
						//get teh size of the character
						string strSubString = "" + text[i];
						textSize = Font.MeasureString(strSubString) * scale;
						position.X -= (textSize.X / 2.0f);

						//get the kerning too
						position.X -= fKerning / 2.0f;
					}
				}
				break;
			}

			//ok, draw each individual letter
			for (int i = 0; i < text.Length; i++)
			{
				float pulsate = MathHelper.Clamp((ShakeAmount * (float)(Math.Sin(time.CurrentTime * ShakeSpeed))), -ShakeAmount, ShakeAmount);
				if ((i % 2) == 0)
				{
					pulsate *= -1.0f;
				}
				Vector2 pulsingPosition = position;
				pulsingPosition.Y += pulsate;

				string strSubString = "" + text[i];

				//Clamp (because we dont want pure black and white)
				spriteBatch.DrawString(
					Font,
					strSubString,
					pulsingPosition,
					color,
					0,
					Vector2.Zero,
					scale,
					SpriteEffects.None,
					0);

				position.X += Font.MeasureString(strSubString).X * scale;
				position.X += fKerning;
			}

			//return the end of that string
			return position.X + textSize.X;
		}

		#endregion //Methods
	}
}