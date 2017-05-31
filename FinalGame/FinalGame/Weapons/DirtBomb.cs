﻿using System;
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
    class DirtBomb : Weapon
    {
        public DirtBomb(String name, int id) : base(name, id)
        {

        }

        public override void initialiseWeapon()
        {
            this.setAddMaterial(true);
            this.setWeaponDamage(10);
        }
    }
}