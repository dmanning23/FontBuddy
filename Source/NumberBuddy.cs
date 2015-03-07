using System.Diagnostics;
using System.Text;
using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FontBuddyLib
{
	/// <summary>
	/// This is an object for writing numbers.
	/// When the number is changed, it will count up and countdown to the new value.
	/// </summary>
	public class NumberBuddy : IFontBuddy
	{
		#region Properties

		/// <summary>
		/// The current number being displayed
		/// </summary>
		private int StartNumber { get; set; }

		/// <summary>
		/// The current number being displayed
		/// </summary>
		private int TargetNumber { get; set; }

		/// <summary>
		/// Set the number to draw for this dude.
		/// </summary>
		public int Number 
		{
			set 
			{
				//If this isn't the same number, add the difference
				if (value != TargetNumber)
				{
					Add(value - TargetNumber);
				}
			}
		}

		/// <summary>
		/// Thing for drawing normal text
		/// </summary>
		private OutlineTextBuddy NormalFont { get; set; }

		/// <summary>
		/// thing for drawing the text when it changes
		/// </summary>
		private BouncyNumbers BouncyFont { get; set; }

		private SpriteFont _font;

		public SpriteFont Font
		{
			get
			{
				return _font;
			}
			set
			{
				_font = value;
				NormalFont.Font = value;
				BouncyFont.Font = value;
			}
		}

		#endregion //Properties

		#region Methods

		public NumberBuddy()
		{
			NormalFont = new OutlineTextBuddy();
			BouncyFont = new BouncyNumbers()
			{
				Rescale = 1f
			};
		}

		/// <summary>
		/// Change the number this dude displays
		/// </summary>
		/// <param name="startNumber"></param>
		/// <param name="targetNumber"></param>
		public void Start(int startNumber, int targetNumber)
		{
			StartNumber = startNumber;
			TargetNumber = targetNumber;
			BouncyFont.Start(StartNumber, TargetNumber);
		}

		public void Add(int num)
		{
			//set the start to the old target
			StartNumber = TargetNumber;

			//set the target to the new number
			TargetNumber = StartNumber + num;

			//start the bouncy text
			Start(StartNumber, TargetNumber);
		}

		/// <summary>
		/// given a content manager and a resource name, load the resource as a bitmap font
		/// </summary>
		/// <param name="rContent"></param>
		/// <param name="strResource"></param>
		public void LoadContent(ContentManager rContent, string strResource)
		{
			//load font
			if (null == Font)
			{
				Font = rContent.Load<SpriteFont>(strResource);
			}
		}

		public float Write(string text,
			Vector2 position,
			Justify justification,
			float scale,
			Color color,
			SpriteBatch spriteBatch,
			GameClock time)
		{
			if (!BouncyFont.IsDead)
			{
				return BouncyFont.Write(text, position, justification, scale, color, spriteBatch, time);
			}
			else
			{
				StringBuilder str = new StringBuilder();
				str.Append(text);
				str.Append(TargetNumber);
				return NormalFont.Write(str.ToString(), position, justification, scale, color, spriteBatch, time);
			}
		}

		public float Write(string text,
			Point position,
			Justify justification,
			float scale,
			Color color,
			SpriteBatch spriteBatch,
			GameClock time)
		{
			return Write(text, new Vector2(position.X, position.Y), justification, scale, color, spriteBatch, time);
		}

		#endregion //Methods
	}
}