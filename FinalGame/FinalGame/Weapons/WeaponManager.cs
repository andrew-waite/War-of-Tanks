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

namespace FinalGame.WeaponNameSpace
{
    public class WeaponManager
    {
        List<Weapon> weapons = new List<Weapon>();

        List<Weapon> activeWeapons = new List<Weapon>();

        public WeaponManager()
        {
            populateWeaponsArray();
        }

        public void populateWeaponsArray()
        {
            weapons.Add(new DirtBomb("DirtBomb", 0));
            weapons.Add(new Missile("Missile", 1));
            weapons.Add(new Nuke("Nuke", 2));

            foreach (Weapon weapon in weapons)
                weapon.initialiseWeapon();
        }

        public List<Weapon> getWeapons()
        {
            return weapons;
        }
    }
}
