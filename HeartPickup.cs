using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public class HeartPickup
    {
        public Game1 game;
        public float x;
        public float y;
        public float health = 0.1f;

        public bool CULLED = false;
        public HeartPickup(Game1 g, float x =0, float y =0) 
        {
            game = g;
            this.x = x;
            this.y = y;
        }
        public void kill()
        {
            CULLED = true;
        }
        public void draw(SpriteBatch batch, Camera cam, Vector2 basePosition)
        {
            if (CULLED)
                return;
            //Vector2 basePosition = new Vector2( this.x - cam.x, cam.y - this.y);
            //batch.Draw(game.heartTexture, basePosition, Color.White);
            Rectangle rect = new Rectangle((int)(basePosition.X), (int)(basePosition.Y), (int)(game.heartTexture.Width * cam.zoom), (int)(game.heartTexture.Height * cam.zoom));
            batch.Draw(game.heartTexture, rect, Color.White);
        }
    }
}
