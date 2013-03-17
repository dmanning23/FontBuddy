using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FontBuddyLib
{
	/// <summary>
	/// This dude writes the text in big pulsating letters, with the outline below it.
	/// </summary>
	public class PulsateBuddy : ShadowTextBuddy
	{
		#region Members

		/// <summary>
		/// how big the pulsate the text
		/// </summary>
		public float PulsateSize { get; set; }

		/// <summary>
		/// How fast to pulsate the speed
		/// </summary>
		public float PulsateSpeed { get; set; }

		/// <summary>
		/// If the pulsate is turned on/off, it will ease into the pulsating
		/// </summary>
		public bool Selected { get; set; }

		private float m_fSelectionFade;

		#endregion //Members

		/// <summary>
		/// Constructor
		/// </summary>
		public PulsateBuddy() : base()
		{
			PulsateSize = 1.0f;
			PulsateSpeed = 4.0f;
			Selected = true;
			m_fSelectionFade = 0.0f;
		}

		/// <summary>
		/// write something on the screen
		/// </summary>
		/// <param name="strText">the text to write on the screen</param>
		/// <param name="Position">where to write at... either upper left, upper center, or upper right, depending on justication</param>
		/// <param name="eJustification">how to justify the text</param>
		/// <param name="fScale">how big to write.  This is not a point size to draw at, it is a multiple of the default font size!</param>
		/// <param name="myColor">the color to draw the text</param>
		/// <param name="mySpriteBatch">spritebatch to use to render the text</param>
		/// <param name="dTime">Most of the other font buddy classes use time somehow, but can jsut send in 0.0f for this dude or ignoer it</param>
		public override float Write(string strText, Vector2 Position, Justify eJustification, float fScale, Color myColor, SpriteBatch mySpriteBatch, double dTime = 0.0f)
		{
			//First draw the shadow
			ShadowWriter.Write(strText,
				Position,
				eJustification,
				fScale,
				ShadowColor,
				mySpriteBatch,
				dTime);

			//multiply the time by the speed
			dTime *= PulsateSpeed;

			// When the menu selection changes, entries gradually fade between
			// their selected and deselected appearance, rather than instantly
			// popping to the new state.
			double fadeSpeed = dTime * 4.0f;

			if (Selected)
			{
				m_fSelectionFade = (float)Math.Min(m_fSelectionFade + fadeSpeed, 1);
			}
			else
			{
				m_fSelectionFade = (float)Math.Max(m_fSelectionFade - fadeSpeed, 0);
			}

			//Pulsate the size of the text
			float pulsate = PulsateSize * (float)(Math.Sin(dTime) + 1.0);
			float pulseScale = 1 + pulsate * 0.15f * m_fSelectionFade;

			//adjust the y position so it pulsates straight out
			Vector2 adjust = ((Font.MeasureString(strText) * fScale * pulseScale) - (Font.MeasureString(strText) * fScale)) / 2.0f;
			Position.Y -= (adjust.Y / 2.0f);

			//Draw the menu item, with the pulsing
			return DrawText(strText, Position, eJustification, fScale * pulseScale, myColor, mySpriteBatch, dTime);
		}
	}
}

