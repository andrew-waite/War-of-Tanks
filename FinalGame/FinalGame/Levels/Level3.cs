using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using RC_Framework;
using TerrainTest.Utils;
using FinalGame.PlayerManager;
using System.Collections.Generic;

using FinalGame.WeaponNameSpace;
using Microsoft.Xna.Framework.Audio;

namespace FinalGame.Levels
{
    class Level3 : RC_GameStateParent
    {
        Texture2D background, tank, turret, bullet, parachute, backgroundOriginal;

        Rectangle backgroundRectangle = new Rectangle(0, 0, 1400, 800);

        SpriteFont font;
        SpriteFont bold;

        bool bulletActive ; //Added

        bool addMaterial;

        Vector2 rocketPosition;
        Vector2 rocketDirection;

        bool rocketFlying;
        bool firing;

        float rocketScaling = 0.4f;

        Color[] bulletColor; //The bullet
        Color[] color; //The background color map
        Color[] backgroundOriginalColour;
        float rocketAngle; //For the projectile only

        Rectangle bulletRectangle;

        List<Player> players;

        public static LevelManager levelManager;
        WeaponManager weaponManager;

        ParticleSystemModified p;

        Rectangle rectangle = new Rectangle(0, 0, 1400, 800);
        Texture2D tex;

        Rectangle rectangle2 = new Rectangle(0, 0, 1400, 800);

        //ParticleSystem ps;

        TextRenderableFade hitTextFade = null;

        Texture2D hud;

        Texture2D missile_thumbnail;

        Texture2D scrollTex;

        SoundEffect soundWindBlowing;
        SoundEffectInstance soundWindowBlowingInstance;

        SoundEffect launch;
        SoundEffectInstance launchInsatnce;

        SoundEffect hit;
        SoundEffectInstance hitInstance;

        ScrollBackGround scrollBackGround;

        public override void LoadContent()
        {
            this.weaponManager = new WeaponManager();

            this.players = new List<Player>();

            this.bulletActive = false;
            this.addMaterial = false;
            this.rocketFlying = false;

            background = Content.Load<Texture2D>("Textures/Level3/moon");

            this.backgroundOriginal = Content.Load<Texture2D>("Textures/Level3/moon");

            this.bold = Content.Load<SpriteFont>("Fonts/spriteFontBold");

            this.backgroundOriginalColour = new Color[this.backgroundOriginal.Width * this.backgroundOriginal.Height];
            this.backgroundOriginal.GetData(this.backgroundOriginalColour);

            tank = Content.Load<Texture2D>("Textures/tank-custom");

            font = Content.Load<SpriteFont>("Fonts/spritefont1");

            parachute = Content.Load<Texture2D>("Textures/parachute");

            this.scrollTex = Content.Load<Texture2D>("Textures/Level3/background_parallax");

            this.missile_thumbnail = Content.Load<Texture2D>("Textures/missile-thumbnail");

            color = new Color[background.Width * background.Height];
            background.GetData(color);

            turret = Content.Load<Texture2D>("Textures/turret");

            this.tex = Content.Load<Texture2D>("Textures/Particle2");

            this.hud = Content.Load<Texture2D>("Textures/hud2");

            this.scrollBackGround = new ScrollBackGround(this.scrollTex, new Rectangle(0, 0, 1400, 800), this.backgroundRectangle, 1.0f, 1);

            bullet = Content.Load<Texture2D>("Textures/circle2");
            bulletColor = new Color[bullet.Width * bullet.Height];
            bullet.GetData(bulletColor);


            //Reset the modified terrain from the original texture
            this.color = this.backgroundOriginalColour;
            this.background.SetData(this.color);


            //Initialise the players:

            players.Add(new Player(0, tank, new Vector2(195, 0), Color.Green, turret, new Vector2(195 + 37, 21), Color.Green, this.weaponManager));

            players.Add(new Player(1, tank, new Vector2(1000, 0), Color.Blue, turret, new Vector2(1000 + 37, 21), Color.Blue, this.weaponManager));

            players[0].setTurretOrigin(2, 5);
            players[1].setTurretOrigin(2, 5);

            levelManager = new LevelManager(players, gameStateManager);
            levelManager.setCurrentPlayer(players[0]);
            levelManager.setGameStatus(true);

            rocketPosition = new Vector2(levelManager.getCurrentPlayer().getPlayerPosition().X + tank.Width / 2, levelManager.getCurrentPlayer().getPlayerPosition().Y);

            this.p = new ParticleSystemModified(new Vector2(levelManager.getCurrentPlayer().getPlayerPosition().X + (tank.Width / 2), levelManager.getCurrentPlayer().getPlayerPosition().Y), 400, 999, 108);
           // ps = new ParticleSystem(new Vector2(0,0), 400, 999, 167);

           //Start the snow particles
          // simulateSnow();

            //Initialise sounds


            this.launch = Content.Load<SoundEffect>("Sounds/rlaunch");
            this.launchInsatnce = this.launch.CreateInstance();

            this.hit = Content.Load<SoundEffect>("Sounds/single_explosion");
            this.hitInstance = this.hit.CreateInstance();

        }

        public override void UnloadContent()
        {
            this.weaponManager = null;
            this.players = null;
            this.rocketPosition = Vector2.Zero;
            this.bulletColor = null;
            this.hitTextFade = null;



            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            prevKeyState = keyState;
            keyState = Keyboard.GetState();

            if (levelManager.getGameStatus())
            {
                levelManager.update();

                processKeyBoardEvents(keyState, prevKeyState);

                simulateGravity();

                UpdateRocket(gameTime);

               // this.ps.Update(gameTime);

                bulletRectangle = new Rectangle((int)rocketPosition.X, (int)rocketPosition.Y, 16, 16);

                this.scrollBackGround.Update(gameTime);


                //TOOD just for testing; Once implemented turns I will know who's current turn it is and then only have to loop over the other players
                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i] == levelManager.getCurrentPlayer()) continue;

                    if (Utils.TerrainModifier.IntersectPixels(bulletRectangle, bulletColor, players[i].getPlayerRectangle(), players[i].getPlayerColorMap()))
                    {
                        System.Diagnostics.Debug.WriteLine("Hit that thign mutherfcaksks");
                        bulletActive = false;
                        rocketFlying = false;
                        rocketPosition = Vector2.Zero;

                        players[i].setHealth(players[i].getHealth() - levelManager.getCurrentPlayer().getCurrentWeapon().getWeaponDamage());

                        this.showHitInfo(new Vector2(players[i].getPlayerPosition().X - 10, players[i].getPlayerPosition().Y - 10), "-" + players[i].getCurrentWeapon().getWeaponDamage());

                        if (!players[i].isAlive()) levelManager.addDeadPlayer(players[i]);

                        this.p.deActivate();

                        this.launchInsatnce.Stop();
                        this.hitInstance.Play();

                        this.firing = false;

                        if (levelManager.getCurrentPlayer().getID() == 0)
                        {
                            Achievement.tanksKilled += 1;
                        }

                        if (levelManager.getCurrentPlayer().getCurrentWeapon().getWeaponID() == 2)
                        {
                            Achievement.theWorldisDestroyed = true;
                        }

                        levelManager.checkEnd();

                        return;
                    }
                }

                if (Utils.TerrainModifier.IntersectPixels(backgroundRectangle, color, bulletRectangle, bulletColor))
                {
                    // System.Diagnostics.Debug.WriteLine("Position X: " + position.X + " Position.Y: " + position.Y);

                    if (levelManager.getCurrentPlayer().getCurrentWeapon().getAddMaterial())
                    {
                        Utils.TerrainModifier.RebuildTerrain(new Circle(new Vector2(rocketPosition.X + 10, rocketPosition.Y + 10), 25), background, color, Color.Gray);
                        bulletActive = false;
                        rocketFlying = false;
                        rocketPosition = Vector2.Zero;
                        this.firing = false;
                        this.p.deActivate();
                        this.launchInsatnce.Stop();
                        this.hitInstance.Play();


                        levelManager.checkEnd();
                    }
                    else
                    {
                        Utils.TerrainModifier.DestroyCircle(new Circle(new Vector2(rocketPosition.X + 10, rocketPosition.Y + 10), 25), background, color);
                        bulletActive = false;
                        rocketFlying = false;
                        rocketPosition = Vector2.Zero;
                        this.firing = false;
                        this.p.deActivate();
                        this.launchInsatnce.Stop();
                        this.hitInstance.Play();

                        if (levelManager.getCurrentPlayer().getID() == 0)
                        {
                            Achievement.tanksKilled += 1;
                        }

                        if (levelManager.getCurrentPlayer().getCurrentWeapon().getWeaponID() == 2)
                        {
                            Achievement.theWorldisDestroyed = true;
                        }

                        levelManager.checkEnd();
                    }
                }

               if(this.hitTextFade != null) this.hitTextFade.Update(gameTime);
            }
        }

        public double FaceObject(Vector2 position, Vector2 target)
        {
            return (Math.Atan2(levelManager.getCurrentPlayer().getPlayerPosition().Y - levelManager.getPlayer(0).getPlayerPosition().Y, 
                levelManager.getCurrentPlayer().getPlayerPosition().X - levelManager.getPlayer(0).getPlayerPosition().X) * (180 / Math.PI));
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.CornflowerBlue);

            if (levelManager.getGameStatus())
            {
                spriteBatch.Begin();

                spriteBatch.DrawString(font, "AddMaterial: " + addMaterial.ToString(), new Vector2(0, 0), Color.Black);
                spriteBatch.DrawString(font, "Angle: " + levelManager.getCurrentPlayer().getAngle(), new Vector2(0, 25), Color.Black);
                spriteBatch.DrawString(font, "Turrent Pos X: " + levelManager.getCurrentPlayer().getTurretPosition().X + " Turret Pos Y: " + levelManager.getCurrentPlayer().getTurretPosition().Y, new Vector2(0, 50), Color.Black);
                spriteBatch.DrawString(font, "Power: " + levelManager.getCurrentPlayer().getPower(), new Vector2(0, 75), Color.Black);

                spriteBatch.DrawString(font, "Player " + levelManager.getCurrentPlayer().getID() + " Health: " + levelManager.getCurrentPlayer().getHealth(), new Vector2(0, 100), Color.Black);

                spriteBatch.DrawString(font, "Game Active: " + levelManager.getGameStatus(), new Vector2(0, 125), Color.Black);
                spriteBatch.DrawString(font, "Turn Player: " + levelManager.getCurrentPlayer().getID(), new Vector2(0, 150), Color.Black);
                spriteBatch.DrawString(font, "Parachute Visibility: " + levelManager.getCurrentPlayer().getParachuteVisibility(), new Vector2(0, 175), Color.Black);


                spriteBatch.DrawString(font, "Current Weapon: " + levelManager.getCurrentPlayer().getCurrentWeapon().getWeaponName(), new Vector2(0, 200), Color.Black);
                spriteBatch.DrawString(font, "X: " + this.rocketPosition.X + " Y: " + this.rocketPosition.Y, new Vector2(0, 225), Color.Black);

                spriteBatch.DrawString(font, "FPS: " + (1 / gameTime.ElapsedGameTime.TotalSeconds), new Vector2(0, 250), Color.Black);

                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
                this.scrollBackGround.Draw(spriteBatch);
                spriteBatch.End();

                spriteBatch.Begin();

                spriteBatch.Draw(background, backgroundRectangle, Color.White); //Draw the background

                spriteBatch.Draw(this.hud, new Vector2(420, 0), null, Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);

                if (levelManager.getCurrentPlayer().getCurrentWeapon().getWeaponID() == 0)
                    spriteBatch.Draw(levelManager.getCurrentPlayer().getCurrentWeapon().getWeaponTexture(), new Vector2(437, 21), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                else if (levelManager.getCurrentPlayer().getCurrentWeapon().getWeaponID() == 1)
                    spriteBatch.Draw(levelManager.getCurrentPlayer().getCurrentWeapon().getWeaponTexture(), new Vector2(440, 24), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                else if (levelManager.getCurrentPlayer().getCurrentWeapon().getWeaponID() == 2)
                    spriteBatch.Draw(levelManager.getCurrentPlayer().getCurrentWeapon().getWeaponTexture(), new Vector2(437, 21), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);


                spriteBatch.DrawString(this.font, levelManager.getCurrentPlayer().getCurrentWeapon().getWeaponName(), new Vector2(500, 38), Color.White);
                spriteBatch.DrawString(this.font, "Weapon:", new Vector2(500, 12), Color.White);


                
                spriteBatch.DrawString(this.font, "Current Player", new Vector2(655, 16), Color.White);
                spriteBatch.DrawString(this.font, ""+ (levelManager.getCurrentPlayer().getID() + 1), new Vector2(725, 33), Color.White);

                //HERE
                spriteBatch.DrawString(this.font, "A: " + levelManager.getCurrentPlayer().getAngle() + " P: " + levelManager.getCurrentPlayer().getPower(), new Vector2(870, 25), Color.White);

                // spriteBatch.DrawString(this.font, levelManager.getCurrentPlayer().getPlayerPosition() + 140, "Health: " + this.levelManager.getCurrentPlayer().getHealth(), Color)



                //draw the tanks and their turrets, and health
                foreach (Player player in players)
                {
                    spriteBatch.Draw(tank, player.getPlayerPosition(), player.getPlayerColor());
                    spriteBatch.Draw(turret, player.getTurretPosition(), null, Color.White, MathHelper.ToRadians(player.getAngle()), player.getTurretOrigin(), 1, SpriteEffects.None, 0);
                    spriteBatch.DrawString(this.bold, "Player: " + (player.getID() + 1), new Vector2(player.getPlayerPosition().X - 15, player.getPlayerPosition().Y - 50), Color.Red);
                    spriteBatch.DrawString(this.bold, "Health: " + player.getHealth(), new Vector2(player.getPlayerPosition().X - 15, player.getPlayerPosition().Y - 25), Color.Red);
                    if (player.getParachuteVisibility()) spriteBatch.Draw(parachute, player.getParachutePosition(), Color.White);
                }

                this.p.Draw(spriteBatch);

                if (bulletActive) spriteBatch.Draw(bullet, rocketPosition, null, Color.White, rocketAngle, new Vector2(20, 20), rocketScaling, SpriteEffects.None, 1);

                //ps.Draw(spriteBatch);

                if (this.hitTextFade != null) this.hitTextFade.Draw(spriteBatch);


                spriteBatch.End();
            }
        }

        private void UpdateRocket(GameTime gameTime)
        {
            if (rocketFlying)
            {
                if(rocketPosition.X < 0 || rocketPosition.X > 1400)
                {
                    this.rocketFlying = false;
                    this.firing = false;
                    this.bulletActive = false;
                    this.rocketPosition = Vector2.Zero;
                    this.p.deActivate();
                    levelManager.checkEnd();
                    
                }

                Vector2 gravity = new Vector2(0, 1);
                rocketDirection += gravity / 5.0f;
                rocketPosition += rocketDirection;

                rocketAngle = (float)Math.Atan2(rocketDirection.X, -rocketDirection.Y);

                //  this.p.sysPos = new Vector2(this.rocketPosition.X - (this.bullet.Width / 2), this.rocketPosition.Y + (this.bullet.Height /2));
                this.p.sysPos = this.rocketPosition;
                this.p.displayAngleOffset = this.rocketAngle;
                this.p.Update(gameTime);
            }
        }

        public void shoot()
        {
            rocketFlying = true;
            bulletActive = true;

            rocketPosition = new Vector2(levelManager.getCurrentPlayer().getPlayerPosition().X + (tank.Width / 2), levelManager.getCurrentPlayer().getPlayerPosition().Y);
            // rocketPosition.X += 20;
            // rocketPosition.Y -= 10;
            rocketAngle = MathHelper.ToRadians(levelManager.getCurrentPlayer().getAngle() - 270);
            Vector2 up = new Vector2(0, -1);
            Matrix rotMatrix = Matrix.CreateRotationZ(rocketAngle);
            rocketDirection = Vector2.Transform(up, rotMatrix);
            rocketDirection *= levelManager.getCurrentPlayer().getPower() / 2.0f;

            this.reloadSmokeTrail();
        }

        public void processKeyBoardEvents(KeyboardState keysState, KeyboardState prevKeyState)
        {
            if (keyState.IsKeyDown(Keys.W) && !prevKeyState.IsKeyDown(Keys.W))
            {
                levelManager.getCurrentPlayer().setPower(levelManager.getCurrentPlayer().getPower() + 1);
            }

            if (keyState.IsKeyDown(Keys.S) && !prevKeyState.IsKeyDown(Keys.S))
            {
                levelManager.getCurrentPlayer().setPower(levelManager.getCurrentPlayer().getPower() - 1);
            }

            if (keyState.IsKeyDown(Keys.C) && !prevKeyState.IsKeyDown(Keys.C))
            {
                addMaterial = !addMaterial;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                if (levelManager.getCurrentPlayer().getAngle() == 360)
                {
                    levelManager.getCurrentPlayer().setAngle(0);
                }

                levelManager.getCurrentPlayer().setAngle(levelManager.getCurrentPlayer().getAngle() + 1);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                if (levelManager.getCurrentPlayer().getAngle() == 0)
                {
                    levelManager.getCurrentPlayer().setAngle(360);
                }

                levelManager.getCurrentPlayer().setAngle(levelManager.getCurrentPlayer().getAngle() - 1);
            }

            if (keyState.IsKeyDown(Keys.F) && !prevKeyState.IsKeyDown(Keys.F) && !firing)
            {
                shoot();
                this.launchInsatnce.Play();
                this.firing = true;
            }

            if(keysState.IsKeyDown(Keys.P) && !prevKeyState.IsKeyDown(Keys.P))
            {
                gameStateManager.pushLevel(2);
            }

            if(keyState.IsKeyDown(Keys.L) && !prevKeyState.IsKeyDown(Keys.L))
            {
                levelManager.nextTurn();
            }

            if(keyState.IsKeyDown(Keys.H) && !prevKeyState.IsKeyDown(Keys.H))
            {
                levelManager.getCurrentPlayer().setAngle(MathHelper.ToDegrees((float)FaceObject(levelManager.getCurrentPlayer().getPlayerPosition(), levelManager.getPlayer(0).getPlayerPosition())) + 315);
            }

            if (keyState.IsKeyDown(Keys.Right) && !prevKeyState.IsKeyDown(Keys.Right))
            {
                int temp = levelManager.getCurrentPlayer().getCurrentWeapon().getWeaponID();
                temp = (temp + 1) % this.weaponManager.getWeapons().Count;

                levelManager.getCurrentPlayer().setCurrentWeapon(this.weaponManager.getWeapons()[temp]);
            }
        }

        public void simulateGravity()
        {
            foreach (Player player in players)
            {
                if (!player.getParachuteVisibility()) continue;
                if (!Utils.TerrainModifier.IntersectPixels(backgroundRectangle, color, player.getPlayerRectangle(), player.getPlayerColorMap()))
                {
                    player.setPlayerPosition(player.getPlayerPosition().X, player.getPlayerPosition().Y + 2);
                    player.setTurretPosition(player.getTurretPosition().X, player.getTurretPosition().Y + 2);
                    if (player.getParachuteVisibility()) player.setParachutePosition(player.getParachutePosition().X, player.getParachutePosition().Y + 2);
                }
                else
                    player.setParachuteVisibility(false);
            }
        }

        public void reloadSmokeTrail()
        {
            p = new ParticleSystemModified(new Vector2(levelManager.getCurrentPlayer().getPlayerPosition().X + (tank.Width / 2), levelManager.getCurrentPlayer().getPlayerPosition().Y), 400, 999, 108);
            p.setMandatory1(tex, new Vector2(6,6), new Vector2(20, 20), levelManager.getCurrentPlayer().getPlayerColor(), new Color(255, 255, 255, 0));
            p.setMandatory2(-1, 1, 1, 1, 0);
            rectangle = new Rectangle(0,0, 1400, 800);
            p.setMandatory3(120, rectangle);
            p.setMandatory4(new Vector2(0,0), new Vector2(0, 0), new Vector2(0, 0));
            p.randomDelta = new Vector2(0.02f, 0.02f);
            p.setDisplayAngle = true;
            p.Origin = 0;
            p.offset = new Vector2(0,0);
           // p.moveTowards = 0;
            //  p.offset = new Vector2(8, 8);
           // p.offset = new Vector2(0,0);
            p.displayAngleOffset = rocketAngle;
            //p.originWayList = new WayPointList();
            //p.originWayList.makePathZigZag(new Vector2(100, 500), new Vector2(700, 500), new Vector2(0, 0), 3, 3);
            //p.moveTowards = 4;
            //p.moveTowardsPos = new Vector2(500, 500);
            p.activate();

        }



       /* public void simulateSnow()
        {
            ps = new ParticleSystem(new Vector2(0, 0), 40, 10000, 102);
            ps.setMandatory1(tex, new Vector2(6, 6), new Vector2(24, 24), Color.White, new Color(255, 255, 255, 100));
            ps.setMandatory2(-1, 1, 1, 1, 0);
            rectangle2 = new Rectangle(-100, 0, 1400, 800);
            ps.setMandatory3(-1, rectangle2);
            ps.setMandatory4(new Vector2(0.1f, 0.1f), new Vector2(1, 0), new Vector2(0,0));
            ps.randomDelta = new Vector2(0.1f, 0.1f);
            ps.Origin = 1;
            ps.originRectangle = new Rectangle(0, 0, 1400, 10);
            ps.activate();
        }*/

        public void showHitInfo(Vector2 positionToDisplay, String text)
        {
            this.hitTextFade = new TextRenderableFade(text, positionToDisplay, font, Color.White, Color.Transparent, 120);
            this.hitTextFade.drift = new Vector2(0f, -0.3f); // make it move up
        }
    }
}
