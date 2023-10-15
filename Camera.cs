using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public class Camera
    {
        public float x = 0f;
        public float y = 0f;
        Game1 game;
        public float zoom = 1f;
        public Camera(Game1 game) 
        {
            this.game = game;
        }
    }
}
