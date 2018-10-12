using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FontBuddyLib
{
	/// <summary>
	/// This dude takes a list of colors and cycles the color of each letter
	/// </summary>
	public class RainbowTextBuddy : ShadowTextBuddy
	{
		#region Members

		/// <summary>
		/// How fast to change the color
		/// Defaults to 2.0
		/// </summary>
		public float RainbowSpeed { get; set; }

		private FontStringCache Text { get; set; }

		#endregion //Members

		#region Properties

		/// <summary>
		/// The list of colors to cycles through
		/// </summary>
		public List<Color> Colors { get; set; }

		private float Kerning
		{
			get; set;
		}

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Cosntructor!
		/// </summary>
		public RainbowTextBuddy()
		{
			RainbowSpeed = 2.0f;
			Colors = new List<Color>();

			//well, this is the rainbow buddy, put some shit in there
			Colors.Add(Color.Purple);
			Colors.Add(Color.Blue);
			Colors.Add(Color.Green);
			Colors.Add(Color.Yellow);
			Colors.Add(Color.Orange);
			Colors.Add(Color.Red);

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

			Kerning = Font.Spacing * scale;

			//update the string cache
			Text.UpdateText(text);

			//Get the correct location
			Vector2 textSize = Font.MeasureString(text) * scale;
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
						position.X -= Kerning;
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
						position.X -= Kerning / 2.0f;
					}
				}
				break;
			}

			WriteShadow(text, position, justification, scale, spriteBatch, time);

			var currentTime = time.CurrentTime;
			currentTime *= RainbowSpeed;
			currentTime += text.Length;
			for (var i = 0; i < Text.StringCache.Count; i++)
			{
				//draw the title

				//Get the current color
				currentTime -= 0.6f;
				var index = (int)currentTime % Colors.Count;

				//get the next color
				var nextIndex = index + 1;
				if (nextIndex >= Colors.Count)
				{
					nextIndex = 0;
				}

				//get the ACTUAL lerped color of this letter
				var remainder = (float)(currentTime - (int)currentTime);
				var letterColor = Color.Lerp(Colors[index], Colors[nextIndex], remainder);

				var subString = Text.StringCache[i];

				spriteBatch.DrawString(
					Font,
					subString,
					position,
					letterColor,
					Rotation,
					Vector2.Zero,
					scale,
					SpriteEffects,
					0);

				position.X += (Font.MeasureString(subString) * scale).X;
				position.X += Kerning;
			}

			//return the end of that string
			return position.X;
		}

		protected override void WriteShadow(string text, 
			Vector2 position, 
			Justify justification, 
			float scale, 
			SpriteBatch spriteBatch, 
			GameClock time)
		{
			for (int i = 0; i < Text.StringCache.Count; i++)
			{
				//draw the title
				var subString = Text.StringCache[i];

				spriteBatch.DrawString(
					Font,
					subString,
					position + ShadowOffset,
					ShadowColor,
					Rotation,
					Vector2.Zero,
					scale * ShadowSize,
					SpriteEffects,
					0);

				position.X += (Font.MeasureString(subString) * scale).X;
				position.X += Kerning;
			}
		}

		#endregion //Methods
	}
}