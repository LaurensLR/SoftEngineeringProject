using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project1.Input;
using System.Collections.Generic;

namespace Project1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _walkTex, _idleTex, _hurtTex, _deathTex;
        private Texture2D _blockTex, _spikeTex;

        private Hero hero;
        private List<ICollidable> worldObjects;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            worldObjects = new List<ICollidable>();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _walkTex = Content.Load<Texture2D>("walk");
            _idleTex = Content.Load<Texture2D>("idle");
            _hurtTex = Content.Load<Texture2D>("hurt");
            _deathTex = Content.Load<Texture2D>("death");
            _spikeTex = Content.Load<Texture2D>("spike");

            _blockTex = new Texture2D(GraphicsDevice, 1, 1);
            _blockTex.SetData(new[] { Color.Red });

            float ground = 400f;

            // Create hero
            hero = new Hero(_walkTex, _idleTex, _hurtTex, _deathTex, new KeyBoardReader(), ground);
            worldObjects.Add(hero);

            // Add blocks
            worldObjects.Add(new Block(_blockTex, new Rectangle(150, (int)ground - 15, 25, 25)));
            worldObjects.Add(new Block(_blockTex, new Rectangle(75, (int)ground - 10, 25, 25)));

            // Add spike
            worldObjects.Add(new Spike(_spikeTex, new Vector2(250, ground - 25)));

        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            hero.Update(gameTime, worldObjects);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            hero.Draw(_spriteBatch);

            foreach (var obj in worldObjects)
            {
                if (obj is Block b) b.Draw(_spriteBatch);
                if (obj is Spike s) s.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
