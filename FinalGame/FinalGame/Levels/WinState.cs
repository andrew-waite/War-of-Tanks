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

using FinalGame.PlayerManager;

using RC_Framework;

namespace FinalGame.Levels
{
    class WinState : RC_GameStateParent
    {
        Texture2D star;
        Texture2D winningPlayer;
        Texture2D looseStar;
        SpriteFont winningFont;

        public static Player wonPlayer;
        public static List<Player> leaderBoard;

        public override void LoadContent()
        {
            this.star = Content.Load<Texture2D>("Textures/win_star_transparent");
            this.winningFont = Content.Load<SpriteFont>("Fonts/BerlandSansFBDemi");
            this.winningPlayer = Content.Load<Texture2D>("Textures/tank-custom");

            this.looseStar = Content.Load<Texture2D>("Textures/you_tried");
        }

        public override void Update(GameTime gameTime)
        {
            prevKeyState = keyState;
            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Escape) && !prevKeyState.IsKeyDown(Keys.Escape))
            {
                gameStateManager.setLevel(1);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.Purple);

            spriteBatch.Begin();

            if (wonPlayer.getID() == 0) WinDraw(this.star);
            else WinDraw(this.looseStar);

            spriteBatch.End();
        }

        public void WinDraw(Texture2D texture)
        {
            spriteBatch.Draw(texture, new Vector2(25, 400 - (texture.Height / 2)), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1);

            spriteBatch.DrawString(this.winningFont, "PLAYER " + (wonPlayer.getID() + 1) + " TRIUMPHS", new Vector2(800, 100), wonPlayer.getPlayerColor());

            spriteBatch.Draw(this.winningPlayer, new Vector2(950, 200), wonPlayer.getPlayerColor());

            for (int i = leaderBoard.Count; i > 0; i--)
            {
                spriteBatch.DrawString(this.winningFont, "SECOND PLACE PLAYER " + (leaderBoard[i - 1].getID() + 1), new Vector2(800, (i * 100) + 300), Color.Black);
                spriteBatch.Draw(leaderBoard[i - 1].getPlayerTexture(), new Vector2(950, (i * 100) + 400), leaderBoard[i - 1].getPlayerColor());
            }
        }
    }
}