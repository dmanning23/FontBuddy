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
<PackageReference Include="FontBuddy" Version="5.*" />
```

## Quick Start

### Basic Text Rendering

```csharp
using FontBuddyLib;
using GameTimer;

// In your Game class
IFontBuddy _fontBuddy;
GameClock _gameClock;

protected override void LoadContent()
{
    _fontBuddy = new FontBuddy();
    _fontBuddy.LoadContent(Content, "Fonts/Arial"); // Your SpriteFont asset
    _gameClock = new GameClock();
}

protected override void Update(GameTime gameTime)
{
    _gameClock.Update(gameTime);
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
        _gameClock         // GameClock for animations
    );
    
    spriteBatch.End();
}
```

### Text Justification

FontBuddy supports multiple justification modes to make alignment easy:

```csharp
// Left-aligned text
_fontBuddy.Write("Left", position, Justify.Left, 1.0f, Color.White, spriteBatch, _gameClock);

// Center-aligned text
_fontBuddy.Write("Center", position, Justify.Center, 1.0f, Color.White, spriteBatch, _gameClock);

// Right-aligned text
_fontBuddy.Write("Right", position, Justify.Right, 1.0f, Color.White, spriteBatch, _gameClock);
```

## Available Font Effects

FontBuddy includes several pre-built text effects:

### ShadowTextBuddy

Renders text with a drop shadow:

```csharp
var shadowFont = new ShadowTextBuddy();
shadowFont.LoadContent(Content, "Fonts/Arial");

// Customize shadow appearance
shadowFont.ShadowOffset = new Vector2(0f, 3f);  // Default offset
shadowFont.ShadowColor = Color.Black;           // Shadow color
shadowFont.ShadowSize = 1.05f;                  // Size multiplier (default 1.05)

shadowFont.Write("Shadowed Text", position, Justify.Center, 1.0f, Color.White, spriteBatch, _gameClock);
```

### PulsateBuddy

Animated pulsating text effect:

```csharp
var pulsateFont = new PulsateBuddy();
pulsateFont.LoadContent(Content, "Fonts/Arial");

// Customize pulsation
pulsateFont.PulsateSize = 1.0f;   // Pulsation amplitude (default 1.0)
pulsateFont.PulsateSpeed = 4.0f;  // Speed of animation (default 4.0)
pulsateFont.StraightPulsate = true; // Pulsate straight out (default true)

// Also has shadow properties (inherits from ShadowTextBuddy)
pulsateFont.ShadowColor = Color.Black;
pulsateFont.ShadowSize = 1.05f;

pulsateFont.Write("Pulsating!", position, Justify.Center, 1.0f, Color.Yellow, spriteBatch, _gameClock);
```

### ShakyTextBuddy

Randomly jittering text for a shaky effect:

```csharp
var shakyFont = new ShakyTextBuddy();
shakyFont.LoadContent(Content, "Fonts/Arial");

shakyFont.ShakeAmount = 10.0f; // Amount of shake (default 10.0)
shakyFont.ShakeSpeed = 10.0f;  // Speed of shake (default 10.0)

shakyFont.Write("Shaky Text", position, Justify.Center, 1.0f, Color.Red, spriteBatch, _gameClock);
```

### RainbowTextBuddy

Text that cycles through rainbow colors character by character:

```csharp
var rainbowFont = new RainbowTextBuddy();
rainbowFont.LoadContent(Content, "Fonts/Arial");

rainbowFont.RainbowSpeed = 2.0f; // Speed of color cycling (default 2.0)

// You can customize the color list
rainbowFont.Colors.Clear();
rainbowFont.Colors.Add(Color.Red);
rainbowFont.Colors.Add(Color.Orange);
rainbowFont.Colors.Add(Color.Yellow);
// ... add more colors

// Also has shadow properties
rainbowFont.ShadowColor = Color.Black;
rainbowFont.ShadowOffset = new Vector2(0f, 3f);

rainbowFont.Write("Rainbow!", position, Justify.Center, 1.0f, Color.White, spriteBatch, _gameClock);
```

### OppositeTextBuddy

Text where colors swap/transition between foreground and shadow over time:

```csharp
var oppositeFont = new OppositeTextBuddy();
oppositeFont.LoadContent(Content, "Fonts/Arial");

oppositeFont.SwapSpeed = 2.0f;  // Speed of color swap (default 2.0)
oppositeFont.SwapSweep = 0.1f;  // Time offset per character (default 0.1)

// Shadow properties
oppositeFont.ShadowColor = Color.Black;
oppositeFont.ShadowOffset = new Vector2(0f, 3f);
oppositeFont.ShadowSize = 1.05f;

oppositeFont.Write("Opposite", position, Justify.Center, 1.0f, Color.Blue, spriteBatch, _gameClock);
```

### OutlineTextBuddy

Text with an outline/stroke effect:

```csharp
var outlineFont = new OutlineTextBuddy();
outlineFont.LoadContent(Content, "Fonts/Arial");

outlineFont.OutlineColor = Color.Black;    // Outline color
outlineFont.OutlineSize = 5;               // Outline thickness in pixels (default 5)
outlineFont.ShadowOffset = new Vector2(0f, 3f);

outlineFont.Write("Outlined", position, Justify.Center, 1.0f, Color.White, spriteBatch, _gameClock);
```

### NumberBuddy

Animated number counter that smoothly transitions when the value changes:

```csharp
var numberFont = new NumberBuddy(0); // Start at 0
numberFont.LoadContent(Content, "Fonts/Arial");

// Set outline properties (uses OutlineTextBuddy internally)
numberFont.OutlineColor = Color.Black;

// Add to the number (animates the change)
numberFont.Add(100); // Counts from current value to current+100

// Or set directly
numberFont.Number = 500; // Counts to 500

numberFont.Write("Score: ", position, Justify.Center, 1.0f, Color.White, spriteBatch, _gameClock);
```

### BouncyNumbers

Specialized number display that counts up and then bounces/scales at the end:

```csharp
var bouncyFont = new BouncyNumbers();
bouncyFont.LoadContent(Content, "Fonts/Arial");

bouncyFont.ScaleAtEnd = 2.5f;   // How big to scale at end (default 2.5)
bouncyFont.ScaleTime = 1.0f;    // How long to scale (default 1.0)
bouncyFont.Rescale = 1.2f;      // Final scale after bounce (default 1.2)
bouncyFont.ScalePause = 1.0f;   // Pause before scaling (default 1.0)

// Start counting from startNum to targetNum
bouncyFont.Start(0, 100);

// Check if animation is done
if (bouncyFont.IsDead)
{
    // Animation finished
}

bouncyFont.Write("Bonus: ", position, Justify.Center, 1.0f, Color.Yellow, spriteBatch, _gameClock);
```

## Core API Reference

All FontBuddy classes implement the `IFontBuddy` interface and share this common API:

### LoadContent
```csharp
void LoadContent(ContentManager content, string resourceName, bool useFontBuddyPlus = false, int fontSize = 24)
```
Loads a SpriteFont for rendering.
- `content` - ContentManager to load from
- `resourceName` - Path to the font asset (e.g., "Fonts/Arial")
- `useFontBuddyPlus` - Use FontBuddyPlus (FontStashSharp) instead of SpriteFont (optional, experimental)
- `fontSize` - Font size for FontBuddyPlus (optional, default 24)

### Write
```csharp
float Write(
    string text, 
    Vector2 position, 
    Justify justify, 
    float scale, 
    Color color, 
    SpriteBatch spriteBatch, 
    GameClock time
)
```

**Parameters:**
- `text` - The string to render
- `position` - Screen position to draw at (alignment depends on justification)
- `justify` - Text alignment: `Justify.Left`, `Justify.Center`, or `Justify.Right`
- `scale` - Size multiplier (1.0 = normal size, 2.0 = double size, etc.)
- `color` - Text color (may be modified by effects)
- `spriteBatch` - MonoGame SpriteBatch to render with
- `time` - GameClock for animations (from GameTimer library)

**Returns:** The X position at the end of the drawn text

### MeasureString
```csharp
Vector2 MeasureString(string text)
```
Returns the size in pixels of the given text.

### Helper Methods

```csharp
// Break text into multiple lines that fit within a width
List<string> BreakTextIntoList(string text, int rowWidth)

// Calculate scale needed to fit text exactly within width
float ScaleToFit(string text, int rowWidth)

// Calculate scale to shrink text to fit (returns 1.0 if already fits)
float ShrinkToFit(string text, int rowWidth)

// Check if text needs to shrink to fit at current scale
bool NeedsToShrink(string text, float scale, int rowWidth)

// Draw text directly without justification handling
void DrawString(string text, Vector2 position, float scale, Color color, SpriteBatch spriteBatch)
```

### Properties

```csharp
SpriteEffects SpriteEffects { get; set; }  // Flip text horizontally/vertically
float Rotation { get; set; }                // Rotation angle in radians
float Spacing { get; }                      // Character spacing from the font
```

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

2. **Add GameTimer dependency:** FontBuddy requires the GameTimer library for the `GameClock` parameter
   ```xml
   <PackageReference Include="GameTimer" Version="*" />
   ```

3. **Common pattern:**
   ```csharp
   using FontBuddyLib;
   using GameTimer;
   
   IFontBuddy font = new FontBuddy(); // or ShadowTextBuddy, PulsateBuddy, etc.
   GameClock clock = new GameClock();
   
   font.LoadContent(Content, "Fonts/YourFont");
   
   // In Update:
   clock.Update(gameTime);
   
   // In Draw:
   font.Write(text, position, Justify.Center, scale, color, spriteBatch, clock);
   ```

4. **All available classes:**
   - `FontBuddy` - Base text rendering
   - `ShadowTextBuddy` - Drop shadow effect
   - `PulsateBuddy` - Size animation (inherits shadow)
   - `ShakyTextBuddy` - Random jitter
   - `RainbowTextBuddy` - Character-by-character color cycling (has shadow)
   - `OppositeTextBuddy` - Foreground/shadow color swapping (has shadow)
   - `OutlineTextBuddy` - Stroke/outline effect
   - `NumberBuddy` - Animated number counter (uses outline)
   - `BouncyNumbers` - Counting number with bounce effect (uses outline)

5. **Key enum:** `Justify` with values: `Left`, `Center`, `Right`

6. **Important interfaces:**
   - `IFontBuddy` - Main interface all font buddies implement
   - `IShadowTextBuddy` - Interface for shadow properties (ShadowColor, ShadowOffset, ShadowSize)

7. **Namespace:** `FontBuddyLib`

8. **Key pattern - GameClock is required:**
   The `Write` method signature is:
   ```csharp
   float Write(string text, Vector2 position, Justify justify, float scale, 
               Color color, SpriteBatch spriteBatch, GameClock time)
   ```
   Note: It's `GameClock time`, NOT `GameTime time`. You must use the GameTimer library's `GameClock` class.
