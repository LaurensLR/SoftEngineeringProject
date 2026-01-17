using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project1.Input;

namespace Project1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Hero _hero;
        private LevelManager _levelManager;
        private UIManager _uiManager;

        /* 
         * FLEXIBLE RESOLUTION: We define a "Virtual" resolution (our design size).
         * Every logic calculation (collisions, movement) stays based on this 800x480 space.
         */
        private const int VirtualWidth = 800;
        private const int VirtualHeight = 480;
        private Matrix _scaleMatrix;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            /* 
             * CONFIGURATION: Setting the game to Fullscreen.
             * We use the adapter's current resolution to match the user's monitor.
             */
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.HardwareModeSwitch = false; // Better for modern Windows (borderless)
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            CalculateMatrix();
            base.Initialize();
        }

        /* 
         * DESIGN PATTERN - Strategy/Transformation:
         * This method calculates how to stretch our 800x480 virtual world to fit any screen.
         */
        private void CalculateMatrix()
        {
            float scaleX = (float)GraphicsDevice.Viewport.Width / VirtualWidth;
            float scaleY = (float)GraphicsDevice.Viewport.Height / VirtualHeight;
            
            // We use the smaller scale to maintain aspect ratio (Letterboxing)
            float scale = MathHelper.Min(scaleX, scaleY);

            _scaleMatrix = Matrix.CreateScale(scale, scale, 1f);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var walk = Content.Load<Texture2D>("walk");
            var idle = Content.Load<Texture2D>("idle");
            var hurt = Content.Load<Texture2D>("hurt");
            var death = Content.Load<Texture2D>("death");
            var spike = Content.Load<Texture2D>("spike");
            var font = Content.Load<SpriteFont>("font");

            var block = new Texture2D(GraphicsDevice, 1, 1);
            block.SetData(new[] { Color.Red });

            /* 
             * DESIGN PATTERN - Factory Pattern:
             * Creating the level via a factory to keep object creation logic separate (SRP).
             */
            var factory = new LevelObjectFactory(block, spike);
            _levelManager = new LevelManager(factory);

            /* 
             * SOLID - Dependency Inversion:
             * Passing the Manager into the Hero so it can interact with the world context.
             */
            _hero = new Hero(walk, idle, hurt, death, new KeyBoardReader(), _levelManager)
            {
                Position = new Vector2(100, 100)
            };

            // Initialize the Observer
            _uiManager = new UIManager(font, _hero);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            _levelManager.Update(gameTime);
            _hero.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black); // Black background for letterbox edges

            /* 
             * SOLID - Open/Closed Principle:
             * We provide the _scaleMatrix to the SpriteBatch. 
             * Now, every object (Hero, Blocks) is drawn normally at their 800x480 
             * coordinates, but the GPU automatically scales them to the full screen.
             */
            _spriteBatch.Begin(transformMatrix: _scaleMatrix);

            _levelManager.Draw(_spriteBatch);
            _hero.Draw(_spriteBatch);
            _uiManager.Draw(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
