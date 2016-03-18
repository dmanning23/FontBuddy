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
		}

		public override float Write(string text,
			Vector2 position,
			Justify justification,
			float scale,
			Color color,
			SpriteBatch spriteBatch,
			GameClock time)
		{
			Kerning = Font.Spacing * scale;

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
						string subString = text[i].ToString();
						textSize = Font.MeasureString(subString) * scale;
						position.X -= textSize.X;

						//get the kerning too
						position.X -= Kerning;
					}
				}
				break;

				case Justify.Center:
				{
					//move teh x value
					for (int i = 0; i < text.Length; i++)
					{
						//get teh size of the character
						string subString = text[i].ToString();
						textSize = Font.MeasureString(subString) * scale;
						position.X -= (textSize.X / 2.0f);

						//get the kerning too
						position.X -= Kerning / 2.0f;
					}
				}
				break;
			}

			WriteShadow(text, position, justification, scale, spriteBatch, time);

			float currentTime = time.CurrentTime;
			currentTime *= RainbowSpeed;
			currentTime += text.Length;
			for (int i = 0; i < text.Length; i++)
			{
				//draw the title

				//Get the current color
				currentTime -= 0.6f;
				int index = (int)currentTime % Colors.Count;

				//get the next color
				int nextIndex = index + 1;
				if (nextIndex >= Colors.Count)
				{
					nextIndex = 0;
				}

				//get the ACTUAL lerped color of this letter
				var remainder = (float)(currentTime - (int)currentTime);
				Color letterColor = Color.Lerp(Colors[index], Colors[nextIndex], remainder);

				string subString = text[i].ToString();

				spriteBatch.DrawString(
					Font,
					subString,
					position,
					letterColor,
					0,
					Vector2.Zero,
					scale,
					SpriteEffects.None,
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
			for (int i = 0; i < text.Length; i++)
			{
				//draw the title
				string subString = text[i].ToString();

				spriteBatch.DrawString(
					Font,
					subString,
					position + ShadowOffset,
					ShadowColor,
					0,
					Vector2.Zero,
					scale * ShadowSize,
					SpriteEffects.None,
					0);

				position.X += (Font.MeasureString(subString) * scale).X;
				position.X += Kerning;
			}
		}

		#endregion //Methods
	}
}