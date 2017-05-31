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
using Microsoft.Xna.Framework.Media;

using RC_Framework;

namespace FinalGame.Levels
{
    class Instructions : RC_GameStateParent
    {
        Texture2D background;
        Texture2D instructions;
        Texture2D transparency_layer;
        Texture2D pageTwo;

        bool second = false;

        public override void LoadContent()
        {
            this.background = Content.Load<Texture2D>("Textures/MainMenu/main_menu_pic");
            this.transparency_layer = Content.Load<Texture2D>("Textures/Instructions/instructions_transparency");
            this.instructions = Content.Load<Texture2D>("Textures/Instructions/Instructions");
            this.pageTwo = Content.Load<Texture2D>("Textures/Instructions/instructions_page_two");
        }

        public override void Update(GameTime gameTime)
        {
            prevKeyState = keyState;
            keyState = Keyboard.GetState();

            if(keyState.IsKeyDown(Keys.Escape) && !prevKeyState.IsKeyDown(Keys.Escape))
            {
                gameStateManager.popLevel();
            }

            if(keyState.IsKeyDown(Keys.Right) && !prevKeyState.IsKeyDown(Keys.Right))
            {
                this.second = !second;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.Blue);

            spriteBatch.Begin();
            spriteBatch.Draw(this.background, Vector2.Zero, Color.White);
            spriteBatch.Draw(this.transparency_layer, Vector2.Zero, Color.White);

            if(!second)
                spriteBatch.Draw(this.instructions, Vector2.Zero, Color.White);
            else
                spriteBatch.Draw(this.pageTwo, Vector2.Zero, Color.White);

            spriteBatch.End();
        }
    }
}
