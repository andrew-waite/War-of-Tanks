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

using FinalGame.WeaponNameSpace;

namespace FinalGame.PlayerManager
{
    public class Player
    {
        Texture2D playerTexture;
        Texture2D turretTexture;

        Vector2 playerPosition;
        Vector2 playerOrigin;

        Color playerColor;
        Color[] playerColorMap;


        Vector2 turretPosition;
        Vector2 turretOrigin;

        Color turretColor;

        WeaponManager weaponManager;
        private Weapon currentWeapon;

        float angle;
        float power;

        Vector2 parachutePosition;
        bool displayParachute;

        int id;
        int health;

        public Player(int number, Texture2D playerTexture, Vector2 playerPosition, Color playerColor, Texture2D turretTexture, Vector2 turretPosition, Color turretColor, WeaponManager weaponManager)
        {

            this.id = number;
            this.playerTexture = playerTexture;
            this.turretTexture = turretTexture;

            this.playerPosition = playerPosition;
            this.playerOrigin = Vector2.Zero;

            this.turretPosition = turretPosition;
            this.turretOrigin = Vector2.Zero;

            this.playerColor = playerColor;
            this.turretColor = turretColor;

            this.parachutePosition = new Vector2(this.playerPosition.X - 20, this.playerPosition.Y - 125);
            this.displayParachute = true;

            this.angle = 0f;
            this.power = 10.00f;

            this.health = 100;

            this.weaponManager = weaponManager;
            this.currentWeapon = this.weaponManager.getWeapons()[1];

            //Populate the playerColorMap
            this.playerColorMap = new Color[playerTexture.Width * playerTexture.Height];
            playerTexture.GetData(this.playerColorMap);
        }

        public void setPlayerOrigin(float x, float y)
        {
            this.playerOrigin = new Vector2(x, y);
        }

        public void setTurretOrigin(float x, float y)
        {
            this.turretOrigin = new Vector2(x, y);
        }

        public void setPlayerPosition(float x, float y)
        {
            this.playerPosition = new Vector2(x, y);
        }

        public void setParachutePosition(float x, float y)
        {
            this.parachutePosition = new Vector2(x, y);
        }

        public void setTurretPosition(float x, float y)
        {
            this.turretPosition = new Vector2(x, y);
        }

        public void setCurrentWeapon(Weapon weapon)
        {
            this.currentWeapon = weapon;
        }

        public void setPlayerColor(Color color)
        {
            this.playerColor = color;
        }

        public void setAngle(float angle)
        {
            this.angle = angle;
        }

        public void setPower(float power)
        {
            this.power = power;
        }

        public void setTurretColor(Color color)
        {
            this.turretColor = color;
        }

        public void setHealth(int health)
        {
            this.health = health;
        }

        public void setParachuteVisibility(bool visibility)
        {
            this.displayParachute = visibility;
        }

        public Vector2 getPlayerOrigin()
        {
            return this.playerOrigin;
        }

        public Vector2 getTurretOrigin()
        {
            return this.turretOrigin;
        }

        public Vector2 getPlayerPosition()
        {
            return this.playerPosition;
        }

        public Vector2 getParachutePosition()
        {
            return this.parachutePosition;
        }

        public Vector2 getTurretPosition()
        {
            return this.turretPosition;
        }

        public Color getPlayerColor()
        {
            return this.playerColor;
        }

        public Color getTurretColor()
        {
            return this.turretColor;
        }

        public Color[] getPlayerColorMap()
        {
            return this.playerColorMap;
        }

        public float getAngle()
        {
            return this.angle;
        }

        public float getPower()
        {
            return this.power;
        }

        public int getHealth()
        {
            return this.health;
        }

        public Rectangle getPlayerRectangle()
        {
            return new Rectangle((int)this.playerPosition.X, (int)this.playerPosition.Y, this.playerTexture.Width, this.playerTexture.Height);
        }

        public Texture2D getPlayerTexture()
        {
            return this.playerTexture;
        }

        public bool isAlive()
        {
            return this.health <= 0 ? false : true;
        }

        public int getID()
        {
            return this.id;
        }

        public bool getParachuteVisibility()
        {
            return this.displayParachute;
        }

        public Weapon getCurrentWeapon()
        {
            return this.currentWeapon;
        }
    }
}
