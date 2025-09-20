using FontBuddyLib;
using GameTimer;
using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace FontBuddySample
{
	/// <summary>
	/// This game tries to show some of the rotated text.
	/// </summary>
	public class RotatedTextGame : Microsoft.Xna.Framework.Game
	{
		#region Properties

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		GameClock CurrentTime;

		IFontBuddy buddy;

		#endregion //Properties

		#region Methods

		public RotatedTextGame()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;
			Content.RootDirectory = "Content";

#if ANDROID
			graphics.PreferredBackBufferWidth = 853;
			graphics.PreferredBackBufferHeight = 480;
#else
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;
#endif

			CurrentTime = new GameClock();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			buddy = new OutlineTextBuddy
			{
				OutlineSize = 1,
				Rotation = MathHelper.ToRadians(90)
			};
			buddy.LoadContent(Content, "ariblk", true, 64);
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
				Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
#if !__IOS__
				Exit();
#endif
			}

			CurrentTime.Update(gameTime);

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();

			//get the start point
			Rectangle screen = graphics.GraphicsDevice.Viewport.TitleSafeArea;
			Vector2 position = new Vector2(screen.Left + 64, screen.Center.Y);

			string test = "Fontbuddy!";

			//draw the left justified text
			position = LineFormatter.GetRotate90JustifiedPosition(test, position, buddy, Justify.Left);
			buddy.Write(test, position, Justify.Left, 1.0f, Color.White, spriteBatch, CurrentTime);

			//draw the centered text
			position.X = screen.Center.X;
			position = LineFormatter.GetRotate90JustifiedPosition(test, position, buddy, Justify.Center);
			buddy.Write(test, position, Justify.Left, 1.0f, Color.White, spriteBatch, CurrentTime);

			//draw the right justified text
			position.X = screen.Right - 64;
			position = LineFormatter.GetRotate90JustifiedPosition(test, position, buddy, Justify.Right);
			buddy.Write(test, position, Justify.Left, 1.0f, Color.White, spriteBatch, CurrentTime);

			spriteBatch.End();

			base.Draw(gameTime);
		}

		#endregion //Methods
	}
}
