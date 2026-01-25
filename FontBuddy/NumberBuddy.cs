using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Text;

namespace FontBuddyLib
{
	/// <summary>
	/// A specialized text renderer for displaying numbers with animated count-up/count-down transitions.
	/// When the number changes, it animates from the old value to the new value with a bouncy scale effect.
	/// </summary>
	/// <remarks>
	/// This class combines two renderers: a <see cref="BouncyNumbers"/> for animated transitions
	/// and an <see cref="OutlineTextBuddy"/> for static display. When the number changes, the bouncy
	/// animation plays; once complete, the number is displayed statically.
	/// </remarks>
	public class NumberBuddy : IFontBuddy
	{
		#region Properties

		/// <summary>
		/// Gets or sets the starting number for the current animation.
		/// </summary>
		private int StartNumber { get; set; }

		/// <summary>
		/// Gets or sets the target number that the animation is counting towards.
		/// </summary>
		private int TargetNumber { get; set; }

		/// <summary>
		/// Sets the number to display. If different from the current target, triggers a count animation.
		/// </summary>
		/// <value>The number to display.</value>
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
		/// Gets or sets the font renderer used for static number display.
		/// </summary>
		private OutlineTextBuddy NormalFont { get; set; } = new OutlineTextBuddy();

		/// <summary>
		/// Gets the font renderer used for animated number transitions.
		/// </summary>
		public BouncyNumbers BouncyFont { get; private set; } = new BouncyNumbers() { Rescale = 1f };

		/// <summary>
		/// Gets or sets whether the pulsation expands from the center horizontally.
		/// </summary>
		/// <value><c>true</c> to expand from center; otherwise, <c>false</c>.</value>
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

		/// <summary>
		/// Gets or sets the sprite effects to apply when rendering.
		/// Setting this value updates both the normal and bouncy font renderers.
		/// </summary>
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

		/// <summary>
		/// Gets or sets the rotation angle in radians.
		/// Setting this value updates both the normal and bouncy font renderers.
		/// </summary>
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
		/// Gets or sets the color of the text outline.
		/// Setting this value updates both the normal and bouncy font renderers.
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

		/// <summary>
		/// Gets the character spacing of the active font renderer.
		/// </summary>
		public float Spacing => !BouncyFont.IsDead ? BouncyFont.Spacing : NormalFont.Spacing;

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="NumberBuddy"/> class with a starting value of 0.
		/// </summary>
		public NumberBuddy()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NumberBuddy"/> class with the specified starting number.
		/// </summary>
		/// <param name="startNum">The initial number to display.</param>
		public NumberBuddy(int startNum)
		{
			StartNumber = startNum;
			TargetNumber = startNum;
		}

		/// <summary>
		/// Releases resources used by this instance, including both font renderers.
		/// </summary>
		public void Dispose()
		{
			NormalFont?.Dispose();
			NormalFont = null;
			BouncyFont?.Dispose();
			BouncyFont = null;
		}

		/// <summary>
		/// Measures the dimensions of the specified text string.
		/// </summary>
		/// <param name="text">The text string to measure.</param>
		/// <returns>A <see cref="Vector2"/> containing the width (X) and height (Y) of the text in pixels.</returns>
		public Vector2 MeasureString(string text)
		{
			return NormalFont.MeasureString(text);
		}

		/// <summary>
		/// Starts an animated count from one number to another.
		/// </summary>
		/// <param name="startNumber">The number to start counting from.</param>
		/// <param name="targetNumber">The number to count to.</param>
		public void Start(int startNumber, int targetNumber)
		{
			StartNumber = startNumber;
			TargetNumber = targetNumber;
			BouncyFont.Start(StartNumber, TargetNumber);
		}

		/// <summary>
		/// Adds a value to the current number and triggers a count animation.
		/// </summary>
		/// <param name="num">The amount to add (can be negative for subtraction).</param>
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
		/// Loads font resources for both the normal and bouncy font renderers.
		/// </summary>
		/// <param name="content">The content manager to load the font from.</param>
		/// <param name="resource">The name of the font resource to load.</param>
		/// <param name="useFontBuddyPlus">If <c>true</c>, uses FontStashSharp; otherwise uses standard SpriteFont.</param>
		/// <param name="fontSize">The font size in points when using FontBuddyPlus.</param>
		public void LoadContent(ContentManager content, string resource, bool useFontBuddyPlus = false, int fontSize = 24)
		{
			NormalFont.LoadContent(content, resource, useFontBuddyPlus, fontSize);
			BouncyFont.LoadContent(content, resource, useFontBuddyPlus, fontSize);
		}

		/// <summary>
		/// Renders text followed by the current number with animation if active.
		/// </summary>
		/// <param name="text">The prefix text to render before the number.</param>
		/// <param name="position">The position to render at. Interpretation depends on justification.</param>
		/// <param name="justification">The text justification mode.</param>
		/// <param name="scale">The scale multiplier for the font size.</param>
		/// <param name="color">The color to render the text in.</param>
		/// <param name="spriteBatch">The SpriteBatch to use for rendering.</param>
		/// <param name="time">The game clock for the animation timing.</param>
		/// <returns>The X position at the end of the rendered text.</returns>
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

		/// <summary>
		/// Draws a text string at the specified position without justification calculations.
		/// </summary>
		/// <param name="text">The text to draw.</param>
		/// <param name="position">The upper-left position to draw at.</param>
		/// <param name="scale">The scale multiplier for the font size.</param>
		/// <param name="color">The color to draw the text in.</param>
		/// <param name="spriteBatch">The SpriteBatch to use for rendering.</param>
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

		/// <summary>
		/// Breaks a text string into multiple lines that fit within the specified width.
		/// </summary>
		/// <param name="text">The text to break into lines.</param>
		/// <param name="rowWidth">The maximum width in pixels for each line.</param>
		/// <returns>A list of strings, each representing a line of text.</returns>
		public List<string> BreakTextIntoList(string text, int rowWidth)
		{
			return LineFormatter.BreakTextIntoList(text, rowWidth, this);
		}

		/// <summary>
		/// Calculates the scale factor needed to make the text exactly fit within the specified width.
		/// </summary>
		/// <param name="text">The text to scale.</param>
		/// <param name="rowWidth">The target width in pixels.</param>
		/// <returns>The scale multiplier to apply to make the text fit the width exactly.</returns>
		public float ScaleToFit(string text, int rowWidth)
		{
			return LineFormatter.ScaleToFit(text, rowWidth, this);
		}

		/// <summary>
		/// Calculates the scale factor needed to shrink the text to fit within the specified width.
		/// Returns 1.0 if the text already fits.
		/// </summary>
		/// <param name="text">The text to potentially shrink.</param>
		/// <param name="rowWidth">The maximum width in pixels.</param>
		/// <returns>The scale multiplier (1.0 or less) to make the text fit.</returns>
		public float ShrinkToFit(string text, int rowWidth)
		{
			return LineFormatter.ShrinkToFit(text, rowWidth, this);
		}

		/// <summary>
		/// Determines whether the text needs to be shrunk to fit within the specified width at the given scale.
		/// </summary>
		/// <param name="text">The text to check.</param>
		/// <param name="scale">The current scale multiplier.</param>
		/// <param name="rowWidth">The maximum width in pixels.</param>
		/// <returns><c>true</c> if the text exceeds the row width at the given scale; otherwise, <c>false</c>.</returns>
		public bool NeedsToShrink(string text, float scale, int rowWidth)
		{
			return LineFormatter.NeedsToShrink(text, scale, rowWidth, this);
		}

		#endregion //Methods
	}
}