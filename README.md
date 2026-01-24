# FontBuddy

**Simple, flexible text rendering for MonoGame with built-in effects and easy justification.**

FontBuddy makes it easy to draw text in MonoGame without wrestling with positioning and effects. It provides simple text rendering with justification options and several built-in visual effects.

## Installation

Install via NuGet:
```bash
dotnet add package FontBuddy
```

Or in your `.csproj`:
```xml
<PackageReference Include="FontBuddy" Version="4.0.0" />
```

## Quick Start

### Basic Text Rendering

```csharp
using FontBuddyLib;

// In your Game class
FontBuddy _fontBuddy;

protected override void LoadContent()
{
    _fontBuddy = new FontBuddy();
    _fontBuddy.LoadContent(Content, "Fonts/Arial"); // Your SpriteFont asset
}

protected override void Draw(GameTime gameTime)
{
    spriteBatch.Begin();
    
    // Draw text with center justification
    _fontBuddy.Write(
        "Hello World!",
        new Vector2(400, 300),
        Justify.Center,
        1.0f,              // scale
        Color.White,
        spriteBatch,
        gameTime
    );
    
    spriteBatch.End();
}
```

### Text Justification

FontBuddy supports multiple justification modes to make alignment easy:

```csharp
// Left-aligned text
_fontBuddy.Write("Left", position, Justify.Left, 1.0f, Color.White, spriteBatch, gameTime);

// Center-aligned text
_fontBuddy.Write("Center", position, Justify.Center, 1.0f, Color.White, spriteBatch, gameTime);

// Right-aligned text
_fontBuddy.Write("Right", position, Justify.Right, 1.0f, Color.White, spriteBatch, gameTime);
```

## Available Font Effects

FontBuddy includes several pre-built text effects:

### ShadowTextBuddy

Renders text with a drop shadow:

```csharp
var shadowFont = new ShadowTextBuddy();
shadowFont.LoadContent(Content, "Fonts/Arial");

// Customize shadow appearance
shadowFont.ShadowOffset = new Vector2(3, 3);
shadowFont.ShadowColor = Color.Black;

shadowFont.Write("Shadowed Text", position, Justify.Center, 1.0f, Color.White, spriteBatch, gameTime);
```

### PulsateBuddy

Animated pulsating text effect:

```csharp
var pulsateFont = new PulsateBuddy();
pulsateFont.LoadContent(Content, "Fonts/Arial");

// Customize pulsation
pulsateFont.PulsateSize = 0.2f;  // How much to grow/shrink
pulsateFont.PulsateSpeed = 2.0f; // Speed of animation

pulsateFont.Write("Pulsating!", position, Justify.Center, 1.0f, Color.Yellow, spriteBatch, gameTime);
```

### ShakyBuddy

Randomly jittering text for a shaky effect:

```csharp
var shakyFont = new ShakyBuddy();
shakyFont.LoadContent(Content, "Fonts/Arial");

shakyFont.ShakeIntensity = 2.0f; // Amount of shake

shakyFont.Write("Shaky Text", position, Justify.Center, 1.0f, Color.Red, spriteBatch, gameTime);
```

### RainbowBuddy

Text that cycles through rainbow colors:

```csharp
var rainbowFont = new RainbowBuddy();
rainbowFont.LoadContent(Content, "Fonts/Arial");

rainbowFont.ColorSpeed = 1.5f; // Speed of color cycling

rainbowFont.Write("Rainbow!", position, Justify.Center, 1.0f, Color.White, spriteBatch, gameTime);
```

### OppositeBuddy

Text that renders with inverted/opposite colors:

```csharp
var oppositeFont = new OppositeBuddy();
oppositeFont.LoadContent(Content, "Fonts/Arial");

oppositeFont.Write("Opposite", position, Justify.Center, 1.0f, Color.Blue, spriteBatch, gameTime);
```

## Core API Reference

All FontBuddy classes inherit from a base class and share this common interface:

### LoadContent
```csharp
void LoadContent(ContentManager content, string fontAssetName)
```
Loads a SpriteFont for rendering.

### Write
```csharp
void Write(
    string text, 
    Vector2 position, 
    Justify justify, 
    float scale, 
    Color color, 
    SpriteBatch spriteBatch, 
    GameTime gameTime
)
```

**Parameters:**
- `text` - The string to render
- `position` - Screen position to draw at
- `justify` - Text alignment (Left, Center, Right)
- `scale` - Size multiplier (1.0 = normal size)
- `color` - Text color (may be modified by effects)
- `spriteBatch` - MonoGame SpriteBatch to render with
- `gameTime` - Game time for animations

## Examples

See the `/FontBuddySample` folder in this repository for a complete working example showing all the different text effects.

## Requirements

- MonoGame 3.7+
- .NET Standard 2.0+

## Platforms

FontBuddy works on all platforms supported by MonoGame:
- Windows
- macOS  
- Linux
- iOS
- Android
- Xbox
- PlayStation

## License

MIT License - see [LICENSE.txt](LICENSE.txt)

## Contributing

Issues and pull requests welcome! This is a hobby project but I maintain it for my game development.

---

## For LLM/AI Agents

**Quick Integration Guide:**

1. **Add the package:** `<PackageReference Include="FontBuddy" Version="4.0.0" />`

2. **Common pattern:**
   ```csharp
   var font = new FontBuddy(); // or ShadowTextBuddy, PulsateBuddy, etc.
   font.LoadContent(Content, "path/to/font");
   font.Write(text, position, Justify.Center, scale, color, spriteBatch, gameTime);
   ```

3. **All classes:**
   - `FontBuddy` - Base text rendering
   - `ShadowTextBuddy` - Drop shadow effect
   - `PulsateBuddy` - Size animation
   - `ShakyBuddy` - Random jitter
   - `RainbowBuddy` - Color cycling
   - `OppositeBuddy` - Inverted colors

4. **Key enum:** `Justify` with values: `Left`, `Center`, `Right`
