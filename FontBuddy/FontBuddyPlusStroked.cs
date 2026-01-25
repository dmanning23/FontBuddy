using FontStashSharp;
using Microsoft.Xna.Framework.Graphics;

namespace FontBuddyLib
{
    /// <summary>
    /// A variant of <see cref="FontBuddyPlus"/> that renders text with a stroke/outline effect.
    /// Uses FontStashSharp's built-in stroke effect for hardware-accelerated outline rendering.
    /// </summary>
    public class FontBuddyPlusStroked : FontBuddyPlus
    {
        /// <summary>
        /// Gets the size of the outline stroke in pixels.
        /// </summary>
        public int OutlineSize { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FontBuddyPlusStroked"/> class with the specified outline size.
        /// </summary>
        /// <param name="outlineSize">The size of the outline stroke in pixels. Default is 1.</param>
        public FontBuddyPlusStroked(int outlineSize = 1)
        {
            OutlineSize = outlineSize;
        }

        /// <summary>
        /// Creates and configures a FontSystem with stroke effect settings.
        /// </summary>
        /// <param name="device">The graphics device.</param>
        /// <returns>A new FontSystem instance configured for stroked text rendering.</returns>
        protected override FontSystem CreateFontSystem(GraphicsDevice device)
        {
            var settings = new FontSystemSettings
            {
                //Effect = FontSystemEffect.Stroked,
                //EffectAmount = OutlineSize
            };
            //settings.Effect = FontSystemEffect.Stroked;
            return new FontSystem(settings);
        }
    }
}