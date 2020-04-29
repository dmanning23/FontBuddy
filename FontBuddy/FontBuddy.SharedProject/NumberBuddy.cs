using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Text;

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
		private OutlineTextBuddy NormalFont { get; set; } = new OutlineTextBuddy();

		/// <summary>
		/// thing for drawing the text when it changes
		/// </summary>
		public BouncyNumbers BouncyFont { get; private set; } = new BouncyNumbers() { Rescale = 1f };

		public bool StraightPulsate
		{
			get
			{
				return BouncyFont.StraightPulsate;
			}
			set
			{
				BouncyFont.StraightPulsate = value;
			}
		}

		private SpriteEffects _spriteEffects;
		public SpriteEffects SpriteEffects
		{
			get
			{
				return _spriteEffects;
			}
			set
			{
				_spriteEffects = value;
				NormalFont.SpriteEffects = SpriteEffects;
				BouncyFont.SpriteEffects = SpriteEffects;
			}
		}

		private float _rotation;
		public float Rotation
		{
			get
			{
				return _rotation;
			}
			set
			{
				_rotation = value;
				NormalFont.Rotation = _rotation;
				BouncyFont.Rotation = _rotation;
			}
		}

		/// <summary>
		/// color to draw the outline
		/// </summary>
		public Color OutlineColor
		{
			get
			{
				return NormalFont.OutlineColor;
			}
			set
			{
				NormalFont.OutlineColor = value;
				BouncyFont.OutlineColor = value;
			}
		}

		public float Spacing => !BouncyFont.IsDead ? BouncyFont.Spacing : NormalFont.Spacing;

		#endregion //Properties

		#region Methods

		public NumberBuddy()
		{
		}

		public NumberBuddy(int startNum)
		{
			StartNumber = startNum;
			TargetNumber = startNum;
		}

		public Vector2 MeasureString(string text)
		{
			return NormalFont.MeasureString(text);
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
		/// <param name="content"></param>
		/// <param name="resource"></param>
		public void LoadContent(ContentManager content, string resource, bool useFontBuddyPlus = false, int fontSize = 24)
		{
			NormalFont.LoadContent(content, resource, useFontBuddyPlus, fontSize);
			BouncyFont.LoadContent(content, resource, useFontBuddyPlus, fontSize);
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
				var str = new StringBuilder();
				str.Append(text);
				str.Append(TargetNumber);
				return NormalFont.Write(str.ToString(), position, justification, scale, color, spriteBatch, time);
			}
		}

		public void DrawString(string text, Vector2 position, float scale, Color color, SpriteBatch spriteBatch)
		{
			if (!BouncyFont.IsDead)
			{
				BouncyFont.DrawString(text, position, scale, color, spriteBatch);
			}
			else
			{
				NormalFont.DrawString(text, position, scale, color, spriteBatch);
			}
		}

		public List<string> BreakTextIntoList(string text, int rowWidth)
		{
			return LineFormatter.BreakTextIntoList(text, rowWidth, this);
		}

		public float ScaleToFit(string text, int rowWidth)
		{
			return LineFormatter.ScaleToFit(text, rowWidth, this);
		}

		public float ShrinkToFit(string text, int rowWidth)
		{
			return LineFormatter.ShrinkToFit(text, rowWidth, this);
		}

		public bool NeedsToShrink(string text, float scale, int rowWidth)
		{
			return LineFormatter.NeedsToShrink(text, scale, rowWidth, this);
		}

		#endregion //Methods
	}
}