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
        private Texture2D _heroWalkTexture;
        private Texture2D _heroIdleTexture;
        private Texture2D _blockTexture;

        private Hero hero;
        private Rectangle tempBlock;
        private Rectangle tempBlock2;

        private List<Rectangle> _obstacles;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _obstacles = new List<Rectangle>();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            base.Initialize();
            float groundLevel = 400f;
            hero = new Hero(_heroWalkTexture,_heroIdleTexture, new KeyBoardReader(), groundLevel);

            _obstacles.Add(new Rectangle(150, (int)groundLevel-15, 25, 25));
            _obstacles.Add(new Rectangle(75, (int)groundLevel - 10, 25, 25));
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _heroWalkTexture = Content.Load<Texture2D>("walk");
            _heroIdleTexture = Content.Load<Texture2D>("idle");
            _blockTexture = new Texture2D(GraphicsDevice, 1, 1);
            _blockTexture.SetData(new[] { Color.Red });
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            hero.Update(gameTime, _obstacles);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            // TODO: Add your drawing code here
            hero.Draw(_spriteBatch);
            foreach (var obstacle in _obstacles)
            {
                _spriteBatch.Draw(_blockTexture, obstacle, Color.Brown);
            }
            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
