using FontStashSharp;
using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FontBuddyLib
{
    /// <summary>
    /// An advanced font rendering implementation using FontStashSharp.
    /// Provides runtime font loading from TTF/OTF files with dynamic atlas generation.
    /// </summary>
    public class FontBuddyPlus : IFontBuddy, IDisposable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the FontStashSharp FontSystem used for font management and rendering.
        /// </summary>
        public FontSystem FontSystem { get; set; }

        /// <summary>
        /// Gets or sets the SpriteFontBase instance for the current font size.
        /// </summary>
        public SpriteFontBase SpriteFont { get; set; }

        /// <summary>
        /// Gets or sets the XNA SpriteFont. Not supported by FontBuddyPlus.
        /// </summary>
        /// <exception cref="NotImplementedException">Always thrown when accessed.</exception>
        public SpriteFont Font { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Gets or sets the sprite effects. Only <see cref="Microsoft.Xna.Framework.Graphics.SpriteEffects.None"/> is supported.
        /// </summary>
        /// <exception cref="NotImplementedException">Thrown when setting to a value other than None.</exception>
        public SpriteEffects SpriteEffects
        {
            get => SpriteEffects.None;
            set
            {
                if (value != SpriteEffects.None)
                {
                    throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// Gets or sets the rotation angle in radians to apply when rendering text.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Gets the character spacing. Always returns 0 for FontBuddyPlus.
        /// </summary>
        public float Spacing => 0;

        #endregion //Properties

        #region Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="FontBuddyPlus"/> class.
        /// </summary>
        public FontBuddyPlus()
        {
        }

        /// <summary>
        /// Releases resources used by this instance, including the FontSystem.
        /// </summary>
        public void Dispose()
        {
            FontSystem?.Dispose();
            FontSystem = null;
        }

        /// <summary>
        /// Loads a TTF/OTF font resource from the content manager.
        /// </summary>
        /// <param name="content">The content manager to load the font from.</param>
        /// <param name="resourceName">The name of the font resource (TTF/OTF file) to load.</param>
        /// <param name="useFontBuddyPlus">Must be <c>true</c> for this class. Throws an exception if <c>false</c>.</param>
        /// <param name="fontSize">The font size in points to use for rendering.</param>
        /// <exception cref="Exception">Thrown when <paramref name="useFontBuddyPlus"/> is <c>false</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the graphics device service is not available.</exception>
        public void LoadContent(ContentManager content, string resourceName, bool useFontBuddyPlus = true, int fontSize = 24)
        {
            if (!useFontBuddyPlus)
            {
                throw new Exception("FontBuddyPlus.LoadContent was passed useFontBuddyPlus = false");
            }

            if (null == FontSystem)
            {
                var graphicDevice = content.ServiceProvider.GetService(typeof(IGraphicsDeviceService)) as IGraphicsDeviceService;
                if (null == graphicDevice)
                {
                    throw new ArgumentNullException(nameof(IGraphicsDeviceService));
                }

                FontSystem = CreateFontSystem(graphicDevice.GraphicsDevice);

                AddFont(content, resourceName, fontSize);
            }
        }

        /// <summary>
        /// Adds a font to the FontSystem from the content manager.
        /// </summary>
        /// <param name="content">The content manager to load the font from.</param>
        /// <param name="resourceName">The name of the font resource to load.</param>
        /// <param name="fontSize">The font size in points to use for the SpriteFont.</param>
        public void AddFont(ContentManager content, string resourceName, int fontSize = 24)
        {
            var resource = content.Load<byte[]>(resourceName);
            FontSystem.AddFont(resource);
            SpriteFont = FontSystem.GetFont(fontSize);
            FontSystem.DefaultCharacter = '\uFFFD';
            FontSystem.CurrentAtlasFull += DynamicSpriteFont_CurrentAtlasFull;
        }

        /// <summary>
        /// Creates and configures the FontSystem. Can be overridden in derived classes to customize settings.
        /// </summary>
        /// <param name="device">The graphics device.</param>
        /// <returns>A new FontSystem instance.</returns>
        protected virtual FontSystem CreateFontSystem(GraphicsDevice device)
        {
            return new FontSystem();
        }

        /// <summary>
        /// Handles the event when the font atlas is full.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        /// <exception cref="NotImplementedException">Always thrown as atlas overflow is not handled.</exception>
        private void DynamicSpriteFont_CurrentAtlasFull(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Measures the dimensions of the specified text string.
        /// The height is calculated to include both ascenders and descenders.
        /// </summary>
        /// <param name="text">The text string to measure.</param>
        /// <returns>A <see cref="Vector2"/> containing the width (X) and height (Y) of the text in pixels.</returns>
        public Vector2 MeasureString(string text)
        {
            var width = SpriteFont.MeasureString(text).X;
            var height = SpriteFont.MeasureString("yY").Y; //make sure the string height includes both ascender and descender
            return new Vector2(width, height);
        }

        /// <summary>
        /// Renders text to the screen with the specified parameters.
        /// </summary>
        /// <param name="text">The text to render.</param>
        /// <param name="position">The position to render at. Interpretation depends on justification.</param>
        /// <param name="justification">The text justification mode.</param>
        /// <param name="scale">The scale multiplier for the font size.</param>
        /// <param name="color">The color to render the text in.</param>
        /// <param name="spriteBatch">The SpriteBatch to use for rendering.</param>
        /// <param name="time">The game clock. Not used by this class but provided for interface compatibility.</param>
        /// <returns>The X position at the end of the rendered text.</returns>
        public float Write(string text, Vector2 position, Justify justification, float scale, Color color, SpriteBatch spriteBatch, GameClock time)
        {
            //if this thing is empty, dont do anything
            if (string.IsNullOrEmpty(text))
            {
                return position.X;
            }

            position = LineFormatter.JustifiedPosition(text, position, this, justification, scale);
            DrawString(text, position, scale, color, spriteBatch);

            //return the end of that string
            return position.X + (MeasureString(text).X * scale);
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
            //okay, draw the actual string
            spriteBatch.DrawString(SpriteFont, text, position, color, Rotation, Vector2.Zero, new Vector2(scale), 0);
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