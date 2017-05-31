using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace FinalGame.WeaponNameSpace
{
    public abstract class Weapon
    {
        private String weaponName;
        public Texture2D weaponTexture;
        private Color[] colorMap;

        private float weaponScale;
        private int weaponDamage;
        private bool weaponActive;
        private bool addMaterial;
        private int weapondID;

        public Weapon(String weaponName, int id)
        {
            this.weaponName = weaponName;
            this.weaponTexture = Program.game.Content.Load<Texture2D>("Textures/Weapons/" + weaponName);

            //populate the colormap with the weapons texture data
            this.colorMap = new Color[this.weaponTexture.Width * this.weaponTexture.Height];
            this.weaponTexture.GetData(this.colorMap);

            this.weaponScale = 0f;

            this.weaponDamage = 0;

            this.weaponActive = false;

            this.addMaterial = false;

            this.weapondID = id;
        }

        public abstract void initialiseWeapon();

           
        public void setWeaponName(String newWeaponName)
        {
            this.weaponName = newWeaponName;
        }

        public void setWeaponScale(float scale)
        {
            this.weaponScale = scale;
        }

        public void setWeaponActive(bool boolean)
        {
            this.weaponActive = boolean;
        }

        public void setAddMaterial(bool boolean)
        {
            this.addMaterial = boolean;
        }

        public void setWeaponDamage(int amount)
        {
            this.weaponDamage = amount;
        }

        public Texture2D getWeaponTexture()
        {
            return this.weaponTexture;
        }

        public String getWeaponName()
        {
            return this.weaponName;
        }

        public float getWeaponScale()
        {
            return this.weaponScale;
        }

        public int getWeaponDamage()
        {
            return this.weaponDamage;
        }

        public bool getAddMaterial()
        {
            return this.addMaterial;
        }
        
        public bool isActive()
        {
            return this.weaponActive;
        }

        public int getWeaponID()
        {
            return this.weapondID;
        }


    }
}
