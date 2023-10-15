using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Weapons
{
    /*public class Pistol : Weapon
    {
        public Pistol() : base()
        {
            fireType = FIRE_TYPE.SemiAutomatic;
            magazineSize = 5;
            fireRate = 0.1f;
            reloadTime = 2;
        }
    }*/
    public class Weapons
    {
        public Weapon Pistol;
        public void loadWeapons(Game1 game)
        {
            Pistol = new Weapon();
            Pistol.texture = game.pistolTexture;
            Pistol.fireType = FIRE_TYPE.SemiAutomatic;
            Pistol.magazineSize= 5;
            Pistol.fireRate = 0.1f;
            Pistol.reloadTime = 2;
        }
    }
}
