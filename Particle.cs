﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public class Particle
    {

        public Game1 game;
        public float x;
        public float y;
        public float dirX = 0;
        public float dirY = 0;
        public float health = 0.1f;
        public float dist = 0;
        public float range = 50;
        public bool CULLED = false;
        public Particle(Game1 g, float x = 0, float y = 0, float dirX = 1, float dirY = 1, float range = 100)
        {
            game = g;
            this.x = x;
            this.y = y;
            this.dirX = dirX;
            this.dirY = dirY;
            this.range = range;
            game.particles.Add(this);
        }
        public void kill()
        {
            CULLED = true;
        }
        public void think()
        {
            if (CULLED)
            {
                return;
            }
            this.x += dirX;
            this.y += dirY;
            dist += 1;
            if (dist > range)
                kill();
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
