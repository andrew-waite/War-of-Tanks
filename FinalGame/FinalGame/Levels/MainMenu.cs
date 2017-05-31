using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RC_Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace FinalGame.Levels
{
    class MainMenu : RC_GameStateParent
    {
        SpriteFont spriteFont;

        Texture2D background;

        Texture2D title;

        Texture2D strip;

        Texture2D level1;

        Texture2D level1Shadow;

        Texture2D level1Shadow100A;

        Texture2D helpButton;
        Texture2D helpButtonBackground;
        Texture2D helpButtonBackground100A;

        Texture2D level2;
        Texture2D level3;

        Texture2D achievments;
        Texture2D achievementsTransparency;
        Texture2D achievements100A;

        bool inLevel1 = false;
        bool inLevel2 = false;
        bool inLevel3 = false;

        bool inHelp = false;
        bool inAchievements = false;

        Rectangle helpRect = new Rectangle(1175, 675, 195, 120);

        Rectangle level1Rect = new Rectangle(425, 375, 150, 150);
        Rectangle level2Rect = new Rectangle(625, 375, 150, 150);
        Rectangle level3Rect = new Rectangle(825, 375, 150, 150);

        Rectangle achievementsRect = new Rectangle(425, 670, 450, 125);

        SoundEffect music;
        SoundEffectInstance musicInstance;

        SoundEffect click;
        SoundEffectInstance clickInstance;

        public override void LoadContent()
        {
            spriteFont = Content.Load<SpriteFont>("Fonts/SpriteFont1");

            this.background = Content.Load<Texture2D>("Textures/MainMenu/main_menu_pic");

            this.title = Content.Load<Texture2D>("Textures/MainMenu/title");

            this.strip = Content.Load<Texture2D>("Textures/MainMenu/second_strip");

            this.level1 = Content.Load<Texture2D>("Textures/MainMenu/level_1_icon");

            this.level1Shadow = Content.Load<Texture2D>("Textures/MainMenu/iconbackground");

            this.level1Shadow100A = Content.Load<Texture2D>("Textures/MainMenu/level1Shadow100A");

            this.helpButton = Content.Load<Texture2D>("Textures/MainMenu/help_button");
            this.helpButtonBackground = Content.Load<Texture2D>("Textures/MainMenu/help_background");
            this.helpButtonBackground100A = Content.Load<Texture2D>("Textures/MainMenu/help_background_100A");

            this.level2 = Content.Load<Texture2D>("Textures/MainMenu/level_2_icon");
            this.level3 = Content.Load<Texture2D>("Textures/MainMenu/level_3_icon");

            this.achievments = Content.Load<Texture2D>("Textures/MainMenu/achievements_icon");
            this.achievementsTransparency = Content.Load<Texture2D>("Textures/MainMenu/achievements_transparency");
            this.achievements100A = Content.Load<Texture2D>("Textures/MainMenu/achievements_transparency_100A");

            this.music = Content.Load<SoundEffect>("Sounds/mainmenu_music");
            this.musicInstance = this.music.CreateInstance();
            this.musicInstance.IsLooped = true;

            this.musicInstance.Play();

            this.click = Content.Load<SoundEffect>("Sounds/click");
            this.clickInstance = this.click.CreateInstance();

        }

        public override void EnterLevel(int fromLevelNum)
        {
            if (this.musicInstance.State == SoundState.Stopped) this.musicInstance.Play();
            base.EnterLevel(fromLevelNum);
        }

        public override void ExitLevel()
        {
            this.musicInstance.Stop();
           // this.clickInstance.Stop();
            base.ExitLevel();
        }

        public override void Update(GameTime gameTime)
        {
            prevKeyState = keyState;
            keyState = Keyboard.GetState();

            getKeyboardAndMouse();

            this.inLevel1 = this.level1Rect.Contains(new Point((int)mouse_x, (int)mouse_y)) ? true : false;
            this.inLevel2 = this.level2Rect.Contains(new Point((int)mouse_x, (int)mouse_y)) ? true : false;
            this.inLevel3 = this.level3Rect.Contains(new Point((int)mouse_x, (int)mouse_y)) ? true : false;
            this.inHelp = this.helpRect.Contains(new Point((int)mouse_x, (int)mouse_y)) ? true : false;
            this.inAchievements = this.achievementsRect.Contains(new Point((int)mouse_x, (int)mouse_y)) ? true : false;

            if ((currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed))
            {
                if (inLevel1)
                {
                    this.clickInstance.Play();
                    //Goto next level and load contents

                    gameStateManager.getLevel(4).LoadContent();
                    gameStateManager.setLevel(4);
                }

                if(inLevel2)
                {
                    this.clickInstance.Play();

                    gameStateManager.getLevel(6).LoadContent();
                    gameStateManager.setLevel(6);
                }

                if(inLevel3)
                {
                    this.clickInstance.Play();

                    gameStateManager.getLevel(7).LoadContent();
                    gameStateManager.setLevel(7);
                }

                if(inAchievements)
                {
                    this.clickInstance.Play();

                    gameStateManager.pushLevel(8);
                }

                if(inHelp)
                {
                    gameStateManager.pushLevel(5);
                }
            }

        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(this.background, Vector2.Zero, Color.White);
            spriteBatch.Draw(this.strip, new Vector2(0, 58), Color.White);
            spriteBatch.Draw(this.title, new Vector2(700 - (this.title.Width / 2), 60), Color.White);

            if (inLevel1)
                spriteBatch.Draw(this.level1Shadow100A, this.level1Rect, Color.White);
            else
                spriteBatch.Draw(this.level1Shadow, this.level1Rect, Color.White);

            if (inLevel2)
                spriteBatch.Draw(this.level1Shadow100A, this.level2Rect, Color.White);
            else
                spriteBatch.Draw(this.level1Shadow, this.level2Rect, Color.White);

            if (inLevel3)
                spriteBatch.Draw(this.level1Shadow100A, this.level3Rect, Color.White);
            else
                spriteBatch.Draw(this.level1Shadow, this.level3Rect, Color.White);


            if (inHelp)
                spriteBatch.Draw(this.helpButtonBackground100A, this.helpRect, Color.White);
            else
                spriteBatch.Draw(this.helpButtonBackground, this.helpRect, Color.White);

            if (inAchievements)
                spriteBatch.Draw(this.achievements100A, this.achievementsRect, Color.White);
            else
                spriteBatch.Draw(this.achievementsTransparency, this.achievementsRect, Color.White);


            spriteBatch.Draw(this.level1, new Vector2(440, 400), Color.White);

            spriteBatch.Draw(this.level2, new Vector2(640, 400), Color.White);

            spriteBatch.Draw(this.level3, new Vector2(840, 400), Color.White);


            spriteBatch.Draw(this.helpButton, new Vector2(1200, 700), Color.White);

            spriteBatch.Draw(this.achievments, new Vector2(450, 685), Color.White);

            

            spriteBatch.End();
        }
    }
}
