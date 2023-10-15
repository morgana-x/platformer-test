using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Reflection.Metadata.BlobBuilder;

namespace Project1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //public Texture2D ballTexture;

        public Texture2D pistolTexture;

        public Texture2D playerTexture_main;
        public Texture2D playerTexture_jumping;
        public Texture2D playerTexture_walking;
        public Texture2D playerTexture_walking2;

        public Texture2D enemyTexture_main;
        public Texture2D enemyTexture_jumping;
        public Texture2D enemyTexture_walking;
        public Texture2D enemyTexture_walking2;


        public Texture2D heartTexture;

        public Texture2D bulletTexture;

        public Texture2D tileTexture;

        public List<Entity> entities = new List<Entity>();

        public List<Enemy> enemies = new List<Enemy>();

        public List<Block> blocks = new List<Block>();

        public List<Bullet> bullets= new List<Bullet>();
        public Player playerEntity;

        public SpriteFont mainFont;

        public Camera camera;
        public Weapons.Weapons weaponsList = new Weapons.Weapons();
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
        }

        public void LoadEntities()
        {

            blocks.Add(new Block(-50, -20, 480, 8));
            blocks.Add(new Block(-50, 5, 12, 5));
            blocks.Add(new Block(-25, 5, 12, 5));
            blocks.Add(new Block(10, 15, 12, 5));

            playerEntity = new Player(this);
            playerEntity.weapons.Add(weaponsList.Pistol);
            camera = new Camera(this);
            camera.zoom = 1;

            for (int x =0; x < 100; x++) 
            {
                Enemy enemy = new Enemy(this);
                enemy.x = 10 * x - 5;
                enemy.y = 2;
                enemy.weapon = weaponsList.Pistol;
            }
        }

        
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            this.Window.Title = "L game";
            LoadEntities();


        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            //ballTexture = Content.Load<Texture2D>("ball");
            playerTexture_main = Content.Load<Texture2D>("player/main");
            playerTexture_jumping = Content.Load<Texture2D>("player/jumping");
            playerTexture_walking = Content.Load<Texture2D>("player/walking");
            playerTexture_walking2 = Content.Load<Texture2D>("player/walking2");

            enemyTexture_main = Content.Load<Texture2D>("enemy/main");
            enemyTexture_jumping = Content.Load<Texture2D>("enemy/jumping");
            enemyTexture_walking = Content.Load<Texture2D>("enemy/walking");
            enemyTexture_walking2 = Content.Load<Texture2D>("enemy/walking2");

            pistolTexture = Content.Load<Texture2D>("pistol");
            bulletTexture = Content.Load<Texture2D>("bullet");
            tileTexture = Content.Load<Texture2D>("tile1");
            heartTexture = Content.Load<Texture2D>("heart");

            mainFont = Content.Load<SpriteFont>("font");
            weaponsList.loadWeapons(this);
             
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            foreach (Entity entity in entities)
            {
                entity.think();
            }
            foreach (Enemy entity in enemies)
            {
                entity.think();
            }
            foreach (Bullet entity in bullets)
            {
                entity.think();
            }
            playerEntity.think();

            camera.x = playerEntity.x; //+= ((playerEntity.x - camera.x) * 0.25f);
            camera.y = playerEntity.y;//+= ((playerEntity.y - camera.y) * 0.25f);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            Vector2 center = new Vector2(this.Window.ClientBounds.Width/2f, this.Window.ClientBounds.Height/2f);
            _spriteBatch.Begin();
            foreach(Entity entity in entities)
            {
                entity.draw(_spriteBatch, this.camera, center + new Vector2((entity.x - camera.x) * camera.zoom, (camera.y - entity.y) * camera.zoom));
            }
            foreach (Enemy entity in enemies)
            {
                entity.draw(_spriteBatch, this.camera, center + new Vector2((entity.x - camera.x) * camera.zoom, (camera.y - entity.y) * camera.zoom));
            }
            foreach (Block entity in blocks)
            {
                entity.draw(this, _spriteBatch, this.camera, center + new Vector2((entity.x - camera.x) * camera.zoom, (camera.y - entity.y) * camera.zoom));
            }
            foreach (Bullet entity in bullets)
            {
                entity.draw( _spriteBatch, this.camera, center + new Vector2((entity.x - camera.x) * camera.zoom, (camera.y - entity.y) * camera.zoom));
            }
            playerEntity.draw(_spriteBatch, this.camera, center + new Vector2( (playerEntity.x - camera.x) * camera.zoom, (camera.y - playerEntity.y) * camera.zoom));

            float heartScale = 5;
            for (int x =0; x < (playerEntity.health / 1 * 20); x++)
            {
                Microsoft.Xna.Framework.Rectangle rect = new Microsoft.Xna.Framework.Rectangle((int) (x * heartTexture.Width * heartScale), (int) (0), (int)( heartTexture.Width * heartScale), (int)(heartTexture.Height * heartScale));
                _spriteBatch.Draw(heartTexture, rect, Color.White); //, new Vector2(heartScale, heartScale));
            }

            if (playerEntity.health <= 0)
            {
               _spriteBatch.DrawString(mainFont, "You Lose", center, Color.Red);
                //_spriteBatch.GraphicsDevice.b
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}