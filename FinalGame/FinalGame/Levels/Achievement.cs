using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RC_Framework;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FinalGame.Levels
{
    class Achievement : RC_GameStateParent
    {
        Texture2D background;
        Texture2D transparency_layer;

        SpriteFont fonty;

        public static int tanksKilled;
        public static bool theWorldisDestroyed;

        public override void LoadContent()
        {
            this.background = Content.Load<Texture2D>("Textures/MainMenu/main_menu_pic");
            this.transparency_layer = Content.Load<Texture2D>("Textures/Instructions/instructions_transparency");
            this.fonty = Content.Load<SpriteFont>("Fonts/BerlandSansFBDemi");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            prevKeyState = keyState;
            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Escape) && !prevKeyState.IsKeyDown(Keys.Escape))
            {
                gameStateManager.popLevel();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(this.background, Vector2.Zero, Color.White);
            spriteBatch.Draw(this.transparency_layer, Vector2.Zero, Color.White);

            spriteBatch.DrawString(this.fonty, "Destroyed the World: " + ((theWorldisDestroyed) ? "Earned" : "Not Yet Earned"), new Vector2(100, 100), Color.White);
            spriteBatch.DrawString(this.fonty, "Tanks Destroyed: " + tanksKilled + " / 10", new Vector2(100, 150), Color.White);
            spriteBatch.End();
        }
    }
}
