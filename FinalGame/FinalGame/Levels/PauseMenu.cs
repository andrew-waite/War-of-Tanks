using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using RC_Framework;

namespace FinalGame.Levels
{
    class PauseMenu : RC_GameStateParent
    {

        int from = 0;

        Texture2D transparency_layer;
        Texture2D exit;
        Texture2D back;
        Texture2D help;

        Texture2D icon_background;
        Texture2D icon_background_100A;

        Rectangle bacKRect = new Rectangle(603, 200, 195, 120);
        Rectangle helpRect = new Rectangle(603, 350, 195, 120);
        Rectangle exitRect = new Rectangle(603, 500, 195, 120);

        bool inHelp = false;
        bool inExit = false;
        bool inBack = false;

        public override void LoadContent()
        {
            font1 = Content.Load<SpriteFont>("Fonts/SpriteFont1");
            this.transparency_layer = Content.Load<Texture2D>("Textures/Instructions/instructions_transparency");
            this.exit = Content.Load<Texture2D>("Textures/MainMenu/exit");
            this.back = Content.Load<Texture2D>("Textures/Pause/back");
            this.help = Content.Load<Texture2D>("Textures/MainMenu/help_button");

            this.icon_background = Content.Load<Texture2D>("Textures/MainMenu/help_background");
            this.icon_background_100A = Content.Load<Texture2D>("Textures/MainMenu/help_background_100A");
        }

        public override void EnterLevel(int fromLevelNum)
        {
            this.from = fromLevelNum;
            base.EnterLevel(fromLevelNum);
        }

        public override void Update(GameTime gameTime)
        {
            prevKeyState = keyState;
            keyState = Keyboard.GetState();

            getKeyboardAndMouse();

            this.inHelp = this.helpRect.Contains(new Point((int)mouse_x, (int)mouse_y)) ? true : false;
            this.inExit = this.exitRect.Contains(new Point((int)mouse_x, (int)mouse_y)) ? true : false;
            this.inBack = this.bacKRect.Contains(new Point((int)mouse_x, (int)mouse_y)) ? true : false;

            //Return to the previous level
            if (keyState.IsKeyDown(Keys.Escape) && !prevKeyState.IsKeyDown(Keys.Escape))
            {
                gameStateManager.popLevel();
            }

            if ((currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed))
            {
                if (inHelp)
                {
                    gameStateManager.pushLevel(5);
                }

                if(inBack)
                {
                    gameStateManager.popLevel();
                }

                if(inExit)
                {
                    if(from == 4)
                    {
                        Level0.levelManager.setGameStatus(false);
                        gameStateManager.getLevel(from).UnloadContent();
                        gameStateManager.setLevel(1);
                    }

                    if (from == 6)
                    {
                        Level1.levelManager.setGameStatus(false);
                        gameStateManager.getLevel(from).UnloadContent();
                        gameStateManager.setLevel(1);
                    }

                    if (from == 7)
                    {
                        Level3.levelManager.setGameStatus(false);
                        gameStateManager.getLevel(from).UnloadContent();
                        gameStateManager.setLevel(1);
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.Red);

            RC_GameStateParent.gameStateManager.getLevel(this.from).Draw(gameTime);

            spriteBatch.Begin();

            spriteBatch.Draw(this.transparency_layer, Vector2.Zero, Color.White);
            //spriteBatch.DrawString(font1, "Press 'u' to return to level", Vector2.Zero, Color.Black);

            if (inHelp)
                spriteBatch.Draw(this.icon_background_100A, this.helpRect, Color.White);
            else
                spriteBatch.Draw(this.icon_background, this.helpRect, Color.White);

            if (inBack)
                spriteBatch.Draw(this.icon_background_100A, this.bacKRect, Color.White);
            else
                spriteBatch.Draw(this.icon_background, this.bacKRect, Color.White);

            if (inExit)
                spriteBatch.Draw(this.icon_background_100A, this.exitRect, Color.White);
            else
                spriteBatch.Draw(this.icon_background, this.exitRect, Color.White);

            spriteBatch.Draw(this.back, new Vector2(630, 225), Color.White);
            spriteBatch.Draw(this.help, new Vector2(630, 375), Color.White);
            spriteBatch.Draw(this.exit, new Vector2(625, 525), Color.White);

            spriteBatch.End();
        }

    }
}
