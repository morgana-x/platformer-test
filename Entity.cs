using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public class Entity
    {
        public float x = 0;
        public float y = 0;
        public float velocity_x = 0 ;
        public float velocity_y = 0;
        public float speed = 10;
        public float w = 10;
        public float h = 10;

        public Game1 game;
        public void draw( SpriteBatch batch, Camera cam, Vector2 basePosition)
        {
            //Vector2 basePosition = new Vector2( this.x - cam.x, cam.y - this.y);
            batch.Draw(game.playerTexture_main, basePosition, Color.White);
        }

        public void think()
        {
            Debug.WriteLine("down");
            this.x += velocity_x;
            this.y += velocity_y;
            if (velocity_x < 0)
            {
                velocity_x += speed / 10;
            }
            if (velocity_x > 0)
            {
                velocity_x -= speed / 10;
            }
            if (velocity_y < 0)
            {
                velocity_y += speed / 10;
            }
            if (velocity_y > 0)
            {
                velocity_y -= speed / 10;
            }
            this.input();
        }
        public void input()
        {
        }
        public void init()
        {
        }

        public void shoot()
        {

        }
        public Block collisionBlockCheck(List<Block> blocks, float offsetX = 0, float offsetY = 0)
        {
            if (blocks.Count <= 0) return null;
            foreach (Block b in blocks)
            {
                if (b == null)
                    continue;
                if (
                    (
                    this.x + offsetX < b.x + b.w &&
                    this.x + offsetX + this.w > b.x &&
                    this.y + offsetY < b.y + b.h &&
                    this.y + offsetY + this.h > b.y
                    )
                    )

                {
                    //Debug.WriteLine("Hit!");
                    return b;
                }

            }
            return null;
        }



        public Entity(Game1 game)
        {
            game.entities.Add(this);
            this.game = game;
            this.init();
        }
    }
}
