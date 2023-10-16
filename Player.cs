using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
//using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public class Player// : Entity
    {
        public float health = 1;
        public float x { get; set; } = 0;
        public float y { get; set; } = 0;
        public float velocity_x { get; set; } = 0;

        public float velocity_x_target { get; set; } = 0;
        public float velocity_y { get; set; } = 0;
        public float speed { get; set; } = 0.05f;

        public float max_velocity_x = 1;

        public float max_velocity_y = 1;
        public int currentWeapon { get; set; } = 0;

        public bool IN_AIR = false;

        public bool WALKING = false;

        public int walkingIndex = 0;

        public int DIRECTION = 1;

        public int nextWalk = 0;

        public int invincibility = 0;

        public bool PUNCHING = false;
        public int punchLeft = 0;

        public List<Weapons.Weapon> weapons = new List<Weapons.Weapon>();
        public Game1 game;
        public Player(Game1 game) //: base(game)
        {
            //game.entities.Add(this);
            this.game = game;
            this.init();
            this.w = game.playerTexture_main.Width;
            this.h = game.playerTexture_main.Height;
        }
        public Texture2D getSprite()
        {
            if (IN_AIR)
                return game.playerTexture_jumping;
            if (PUNCHING)
                return game.playerTexture_punch;
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
                        return game.playerTexture_walking;
                    case 1:
                        return game.playerTexture_walking2;
                    default:
                        return game.playerTexture_walking;
                }
                return game.playerTexture_walking;
            }

            return game.playerTexture_main;
        }
        public void draw(SpriteBatch batch, Camera cam, Vector2 basePosition)
        {
            // Vector2 basePosition = new Vector2(this.x - cam.x,  cam.y - this.y);
            //batch.Draw(game.playerTexture, basePosition, Color.White); 

            Texture2D sprite = getSprite();
            Microsoft.Xna.Framework.Rectangle rect = new Microsoft.Xna.Framework.Rectangle((int)basePosition.X, (int)basePosition.Y, (int)(sprite.Width * cam.zoom), (int)(sprite.Height * cam.zoom));
            // Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth
            //batch.Draw(sprite, basePosition, rect, Color.White, 0, new Vector2(rect.X, rect.Y), 1, DIRECTION == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0) ;
            //Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth
            batch.Draw(sprite, rect, Color.White); //, 0, new Vector2(rect.X, rect.Y), 1, DIRECTION == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 100);
           // Weapons.Weapon currentWeapon = getCurrentWeapon();
            //if (currentWeapon == null)
            //    return;
           // batch.Draw(currentWeapon.texture, basePosition + new Vector2(2, -0.25f), Color.White);
        }
        public Weapons.Weapon getCurrentWeapon()
        {
            if (weapons.Count <= 0) 
                return null;
            return weapons[currentWeapon];
        }
        public void damage(float damage)
        {
            if ( (damage < 0) && (invincibility > 0))
                return;
            this.health -= damage;
            invincibility = 50;
            /*if (this.health > 1) 
            {
                this.health = 1;
            }*/
        }
        public void think()
        {
            pickUps();
            if (invincibility > 0)
                invincibility--;
            if (punchLeft > 0)
            {
       
                punchLeft--;
                if (punchLeft == 0)
                    PUNCHING = false;
                punchThink();
            }

            WALKING = (velocity_x_target != 0);
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
            this.input();
        }
        public float w = 5;
        public float h = 10;
        public bool collisionCheck(List<Block> blocks, float offsetX =0, float offsetY = 0)
        {
            if (blocks.Count <= 0) return false;
            foreach(Block b in blocks)
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

        public DateTime nextShoot;
        public void input()
        {
            KeyboardState kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.Up) || kstate.IsKeyDown(Keys.W))
            {
                //Block b = collisionBlockCheck(game.blocks);
                if (!IN_AIR)
                    velocity_y += max_velocity_y;
            }
            if (kstate.IsKeyDown(Keys.Down) || kstate.IsKeyDown(Keys.S))
            {
                Block b = collisionBlockCheck(game.blocks);
                if (b == null)
                {
                    velocity_y -= max_velocity_y * 0.5f;

                }
                else
                {
                    if (velocity_y < 0)
                    {
                        velocity_y = 0;
                    }
                    if (y < b.y)
                    {
                        y = b.y;
                    }
                }
                
            }
            if (kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(Keys.A))
            {
                DIRECTION = -1;
                if (!collisionCheck(game.blocks, velocity_x, 0));
                    velocity_x_target = -max_velocity_x;
            }
            else if (kstate.IsKeyDown(Keys.Right) || kstate.IsKeyDown(Keys.D))
            {
                DIRECTION = 1;
                if (!collisionCheck(game.blocks, velocity_x, 0)) ;
                    velocity_x_target = max_velocity_x;
            }
            else
            {
                velocity_x_target = 0;
            }

            if (punched && (kstate.IsKeyUp(Keys.F) && Mouse.GetState().LeftButton == ButtonState.Released))
                punched = false;

            if ((kstate.IsKeyDown(Keys.F) || Mouse.GetState().LeftButton == ButtonState.Pressed) && !punched) 
            {
                if (DateTime.Now > nextShoot)
                {
                    //shoot();
                    punch();
                    nextShoot = DateTime.Now.AddSeconds(0.5f);
                }
            }
        }
        public bool punched = false;
        public void punch()
        {
            PUNCHING = true;
            punchLeft = 5;
      
        }
        public void punchThink()
        {
            foreach (Enemy e in game.enemies)
            {
                if (e.DEAD) continue;
                if (e.CLEARED) continue;
                float dX = (e.x - x);
                bool inFront = (dX < 0 && DIRECTION < 0) || (dX > 0 && DIRECTION > 0 ) || false;
                if (inFront && (Math.Abs((e.x - x)) + Math.Abs(e.y - y) < w * 2f))
                {
                    e.damage(0.5f);
                    e.velocity_x += DIRECTION * 19f;
                    e.x += DIRECTION;
                    e.DIRECTION = DIRECTION;
                    e.velocity_y += 0.7f;
                    game.particleBlast(e.x, e.y, 5);
                }
                //e.DEAD = true;

            }
        }

        public void pickUps()
        {
            if (this.health >= 1)
                return;
            foreach(HeartPickup e in game.heartPickups)
            {
                if (e.CULLED)
                    continue;
                if (Math.Abs(e.x - x) + Math.Abs(e.y -y )  < 5)
                {
                    this.health += e.health;
                    if (this.health > 1)
                        this.health = 1;
                    e.kill();
                }
            }
        }
        public void shoot()
        {
            Bullet bullet = new Bullet(this.game);
            //bullet.init();
            this.game.bullets.Add(bullet);

            MouseState state = Mouse.GetState();
            float dist_x = x - state.X;
            float dist_y = y - state.Y;
            float ang = (float)Math.Tan( (dist_x/dist_y));
            float dir_x = (float)Math.Cos(ang);
            float dir_y = (float)Math.Sin(ang);
            bullet.dir_x= dir_x;
            bullet.dir_y = dir_y;
            bullet.x = x + (dir_x * w * 1.5f);
            bullet.y = y + (dir_x * h * 1.5f);
        }
        public void init()
        {

        }/*
        public Player(Game1 game)
        {
            game.entities.Add(this);
            this.game = game;
            this.init();
        }*/
    }
}
