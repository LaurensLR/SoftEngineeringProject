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

        private Hero _hero;
        private Level _currentLevel;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load textures
            _walkTex = Content.Load<Texture2D>("walk");
            _idleTex = Content.Load<Texture2D>("idle");
            _hurtTex = Content.Load<Texture2D>("hurt");
            _deathTex = Content.Load<Texture2D>("death");
            _spikeTex = Content.Load<Texture2D>("spike");

            _blockTex = new Texture2D(GraphicsDevice, 1, 1);
            _blockTex.SetData(new[] { Color.Red });

            // Create Level
            _currentLevel = new Level(_blockTex, _spikeTex);

            // Create Hero (Spawn him in the air, let him fall to the map)
            _hero = new Hero(_walkTex, _idleTex, _hurtTex, _deathTex, new KeyBoardReader());
            _hero.Position = new Vector2(100, 100);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Pass the level's objects to the hero
            _hero.Update(gameTime, _currentLevel.LevelObjects);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _hero.Draw(_spriteBatch);

            // Draw level objects
            foreach (var obj in _currentLevel.LevelObjects)
            {
                if (obj is Block b) b.Draw(_spriteBatch);
                else if (obj is Spike s) s.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
