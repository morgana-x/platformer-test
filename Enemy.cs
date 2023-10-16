using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Project1
{
    public class Enemy
    {
        public float x = 0;
        public float y = 0;
        public float w = 10;
        public float h = 10;
        public float velocity_x = 0;
        public float velocity_y = 0;
        public float speed = 0.2f;
        public float health = 1;
        public Game1 game;
        public bool WALKING = false;
        public bool IN_AIR = false;
        public int DIRECTION = 1;
        public bool DEAD = false;
        public bool CLEARED = false;
       
        public int z = 1;
        public float velocity_x_target = 0;

        public float max_velocity_x = 0.8f;

        public float max_velocity_y = 1;
        public Weapons.Weapon weapon { get; set; }
        public Enemy(Game1 game)
        {
            game.enemies.Add(this);
            this.game = game;
        }
        public void die()
        {
            DEAD = true;
        }
        public void damage(float damage)
        {
            if (DEAD)
                return;
            health -= damage;
            
            if (health > 1)
            {
                health = 1;
            }
            if (health <= 0)
            {
                die();
            }
        }
        public int nextWalk = 0;
        public int walkingIndex = 0;
        public Texture2D getSprite()
        {
            if (DEAD)
                return game.enemyTexture_jumping;
            if (IN_AIR)
                return game.enemyTexture_jumping;
            if (WALKING)
            {
                nextWalk++;
                if (nextWalk > 5)
                {
                    nextWalk = 0;
                    walkingIndex++;
                    if (walkingIndex > 1)
                        walkingIndex = 0;

                }
                switch (walkingIndex)
                {
                    case 0:
                        return game.enemyTexture_walking;
                    case 1:
                        return game.enemyTexture_walking2;
                    default:
                        return game.enemyTexture_walking;
                }
                return game.enemyTexture_walking;
            }

            return game.enemyTexture_main;
        }
        public void draw(SpriteBatch batch, Camera cam, Vector2 basePosition)
        {
            //Vector2 basePosition = new Vector2( this.x - cam.x, cam.y - this.y);
            //batch.Draw(game.playerTexture, basePosition, Color.White);
            if (CLEARED)
                return;
            float zoom = (z > 1 ?  1 + (z * cam.zoom) : 0);
            batch.Draw(getSprite(), new Rectangle((int)(basePosition.X + (zoom * DIRECTION)), (int)(basePosition.Y + -zoom), (int)(game.playerTexture_main.Width * cam.zoom ), (int)(game.playerTexture_main.Height * cam.zoom)), Color.White);
           // Weapons.Weapon currentWeapon = weapon;// getCurrentWeapon();
            //if (currentWeapon == null)
           //     return;
            //batch.Draw(currentWeapon.texture, basePosition + new Vector2(2, -0.25f), Color.White);
        }
        public void think()
        {
            if (DEAD && !CLEARED)
            {
                z++;
                if (z > 100)
                {
                    CLEARED = true;
                    game.particleBlast(x + z, y + z, 10);
                }
                //game.enemies.Remove(this);
            }

            if (CLEARED)
                return;
            if (DEAD)
                return;
            WALKING = (velocity_x_target != 0);
            if (velocity_x_target > 0)
                DIRECTION = 1;
            else if (velocity_x_target < 0)
                DIRECTION = -1;
            Block b = collisionBlockCheck(game.blocks, 0, -1);
            if (b != null)
            {
                if (velocity_y < 0)
                {
                    velocity_y = 0;
                }
                if (y < b.y)
                {
                    y = b.y;
                }
                IN_AIR = false;
            }
            else
            {
                velocity_y = velocity_y - 0.025f;
                IN_AIR = true;
            }
            float diff = (velocity_x - velocity_x_target);
            if (diff > 0)
            {
                velocity_x -= speed * (IN_AIR ? 0.7f : 1);
                if (diff < 0.05f)
                    velocity_x = velocity_x_target;
            }
            if (diff < 0)
            {
                velocity_x += speed * (IN_AIR ? 0.7f : 1);
                if (diff > -0.05f)
                    velocity_x = velocity_x_target;
            }
            velocity_y = Math.Clamp(velocity_y, -max_velocity_y, max_velocity_y);
            velocity_x = Math.Clamp(velocity_x, -max_velocity_x, max_velocity_x);
            if (velocity_x != 0)
                this.x += collisionCheck(game.blocks, velocity_x, 0) ? 0 : velocity_x;
            if (velocity_y != 0)
                this.y += collisionCheck(game.blocks, 0, velocity_y) ? 0 : velocity_y;

     
            ///////////////////////////
            velocity_x_target = 0;
            float dist = Math.Abs( (game.playerEntity.x - x) + (game.playerEntity.y - y));
            float rqDist = 12;
            if (dist > 50)
                return;
            foreach(Enemy e in game.enemies)
            {
                if (e == this)
                    continue;
                if (e.DEAD || e.CLEARED)
                    continue;
                if (Math.Abs( (e.x - game.playerEntity.x)) < dist) 
                {
                    rqDist += e.w;
                }
            }
            int dir = 1;
            if (game.playerEntity.x - x < 0)
                dir = -1;
            if (dist > rqDist)
            {
                velocity_x_target = dir;
               //x = x + (dir * speed);
            }
            else
            {
                //shoot();
                punch();
            }
   
        
        }
        public void punch()
        {
            Player e = game.playerEntity;
            if (e.invincibility > 0)
                return;
            if (e.health <= 0)
                return;
            if (!collide(game.playerEntity))
                return;
            e.damage(0.05f);
            e.velocity_x += DIRECTION * 19f;
            e.x += DIRECTION;
            e.velocity_y += 0.7f;
            game.particleBlast(e.x, e.y);
            //e.DEAD = true;
        }
        public bool collide(Player b)
        {
            float offsetX = 0;
            float offsetY = 0;
            if ((this.x + offsetX < b.x + b.w &&
             this.x + offsetX + this.w > b.x &&
             this.y + offsetY < b.y + b.h &&
             this.y + offsetY + this.h > b.y)
             /*|| 
             (b.x < this.x + this.w &&
             b.x + b.w > this.x &&
             b.y < this.y + this.h &&
             b.y + b.h > this.y)*/)
            {
                //Debug.WriteLine("Hit!");
                return true;
            }
            return false;
        }
        public bool collisionCheck(List<Block> blocks, float offsetX = 0, float offsetY = 0)
        {
            if (blocks.Count <= 0) return false;
            foreach (Block b in blocks)
            {
                if (b == null)
                    continue;
                if ((this.x + offsetX < b.x + b.w &&
                    this.x + offsetX + this.w > b.x &&
                    this.y + offsetY < b.y + b.h &&
                    this.y + offsetY + this.h > b.y)
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
    }
}
