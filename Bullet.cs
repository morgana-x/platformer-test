using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Project1
{
    public class Bullet
    {
        public float dir_x = 0;
        public float dir_y = 0;
        public float x;
        public float y;
        public Game1 game;

        float w = 1;
        float h = 1;

        public Bullet(Game1 game) //: base(game)
        {
            this.game = game;
            //this.init();
        }
        public void draw(SpriteBatch batch, Camera cam, Vector2 basePosition)
        {
            if (DESTROYED)
                return;
            // Vector2 basePosition = new Vector2(this.x - cam.x,  cam.y - this.y);
            //batch.Draw(game.playerTexture, basePosition, Color.White); 
            batch.Draw(game.bulletTexture, new Rectangle((int)basePosition.X, (int)basePosition.Y, (int)(game.bulletTexture.Width * cam.zoom), (int)(game.bulletTexture.Height * cam.zoom)), Color.White);

        }

        public bool collide(Entity ent)
        {
            if ((this.x < ent.x + ent.w &&
                this.x + this.w > ent.x &&
                this.y < ent.y + ent.h &&
                this.y + this.h > ent.y))
            {
                return true;
            }
            return false;
        }
        public bool collide(Enemy ent)
        {
            if ((this.x < ent.x + ent.w &&
                this.x + this.w > ent.x &&
                this.y < ent.y + ent.h &&
                this.y + this.h > ent.y))
            {
                return true;
            }
            return false;
        }
        public bool collide(Player ent)
        {
            if ((this.x < ent.x + ent.w &&
                this.x + this.w > ent.x &&
                this.y < ent.y + ent.h &&
                this.y + this.h > ent.y))
            {
                return true;
            }
            return false;
        }
        public bool collide(Bullet ent)
        {
            if ((this.x < ent.x + ent.w &&
                this.x + this.w > ent.x &&
                this.y < ent.y + ent.h &&
                this.y + this.h > ent.y))
            {
                return true;
            }
            return false;
        }
        public bool collisionCheck(List<Block> blocks)
        {
            if (blocks.Count <= 0) return false;
            foreach (Block b in blocks)
            {
                if (b == null)
                    continue;
                if ((this.x < b.x + b.w &&
                    this.x + this.w > b.x &&
                    this.y < b.y + b.h &&
                    this.y + this.h > b.y)
                    /*|| 
                    (b.x < this.x + this.w &&
                    b.x + b.w > this.x &&
                    b.y < this.y + this.h &&
                    b.y + b.h > this.y)*/)
                {
                    //Debug.WriteLine("Hit!");
                    return true;
                }

            }
            return false;
        }
        public bool DESTROYED = false;
        public void destroy()
        {
            if (game.bullets.Contains(this))
                DESTROYED = true;
                //game.bullets.Remove(this);
        }
        public float dist = 0;
        public float speed = 0.2f;
        public void think()
        {
            if (DESTROYED)
                return;
            this.x += dir_x * speed;
            this.y += dir_y * speed;
            dist += (dir_x + dir_y);
            if (dist > 20)
                destroy();
            if (collisionCheck(game.blocks))
            {
                destroy();
            }
            if (collide(game.playerEntity))
            {
                game.playerEntity.damage(0.1f);
                destroy();
            }
            foreach(Enemy e in game.enemies)
            {
                if (collide(e))
                {
                    e.damage(0.1f);
                    destroy();
                    break;
                }
            }
            foreach (Bullet e in game.bullets)
            {
                if (collide(e))
                {
                    e.destroy();
                    destroy();
                    break;
                }
            }
        }
    }

  
}
