using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Project1.Weapons
{
    public enum FIRE_TYPE
    {
        Automatic = 0,
        SemiAutomatic = 1,
    }
    public class Weapon
    {
        public Texture2D texture { get; set; }
        public float fireRate { get; set; } = 0.1f;
        public float reloadTime { get; set; } = 2;

        public float magazineSize { get; set; } = 20;

        public float damage { get; set; } = 0.1f;

        public FIRE_TYPE fireType { get; set; } = FIRE_TYPE.Automatic;
    }
}
