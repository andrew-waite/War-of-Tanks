using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using M = Microsoft.Xna.Framework.Media;
using RC_Framework;

namespace FinalGame.Levels
{
    
    class SplashScreen : RC_GameStateParent
    {
        bool timerActive = true;

        Texture2D splashScreen;
        SpriteFont fontymcFontFace;

        TextRenderableFlash flash;

        public override void LoadContent()
        {
            splashScreen = Content.Load<Texture2D>("Textures/SplashScreen/splash_screen");
            this.fontymcFontFace = Content.Load<SpriteFont>("Fonts/BerlandSansFBDemi");
            this.flash = new TextRenderableFlash("Enter to continue...", new Vector2(560, 690), this.fontymcFontFace, Color.White, 25);
        }

        public override void Update(GameTime gameTime)
        {
            prevKeyState = keyState;
            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Enter) && !prevKeyState.IsKeyDown(Keys.Enter))
            {
                gameStateManager.setLevel(1);
            }

            this.flash.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.Red);

            spriteBatch.Begin();
            spriteBatch.Draw(splashScreen, Vector2.Zero, Color.White);
            this.flash.Draw(spriteBatch);
            spriteBatch.End();


        }
    }
}
