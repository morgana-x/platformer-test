using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public class Block
    {
        public int x { get; set; } = 0;
        public int y { get; set; } = 0;
        public int w { get; set; } = 0;
        public int h { get; set; } = 0;

        //public Rectangle rect { get; set; }// = new Rectangle(0,0,0,0);

        public Block(int x, int y, int w, int h)
        {
            //rect.X = x; rect.Y = y;
            //rect.Width= w; rect.Height = h;
            //rect = new Rectangle(x,y,w,h);
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }
 
        public void draw(Game1 game, SpriteBatch batch, Camera cam, Vector2 basePosition)
        {
             batch.Draw(game.tileTexture, new Rectangle((int)basePosition.X, (int) (basePosition.Y + ( (h) * cam.zoom)), (int)(this.w * cam.zoom), (int)(this.h * cam.zoom)), Color.White); //  this.rect, Color.White);
            
            /*float tw = 8; // game.tileTexture.Width
            for ( int tx = 0; tx < w / tw; tx++)
            {
                batch.Draw(
                    
                    game.tileTexture, 

                    new Rectangle(

                        (int) ((basePosition.X ) + (tx * tw * cam.zoom)), //game.tileTexture.Width * cam.zoom)) ,

                        (int)basePosition.Y - this.h / 2, 

                        (int)(tx * tw * cam.zoom),//(game.tileTexture.Width * cam.zoom), 

                        (int)(h * cam.zoom)

                        ),


                    Color.White
                    );
            }*/
        }
    }
}
