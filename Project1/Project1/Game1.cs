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
        
        // Expose components for states to access (DIP)
        public Hero Hero { get; private set; }
        public LevelManager LevelManager { get; private set; }
        public UIManager UiManager { get; private set; }
        public SpriteFont Font { get; private set; }

        private IGameState _currentState;
        private Matrix _scaleMatrix;
        private const int VirtualWidth = 800;
        private const int VirtualHeight = 480;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.HardwareModeSwitch = false; 
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            float scale = MathHelper.Min((float)GraphicsDevice.Viewport.Width / VirtualWidth, (float)GraphicsDevice.Viewport.Height / VirtualHeight);
            _scaleMatrix = Matrix.CreateScale(scale, scale, 1f);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load assets
            var walk = Content.Load<Texture2D>("walk");
            var idle = Content.Load<Texture2D>("idle");
            var hurt = Content.Load<Texture2D>("hurt");
            var death = Content.Load<Texture2D>("death");
            var spike = Content.Load<Texture2D>("spike");
            var cherry = Content.Load<Texture2D>("cherry");
            var door = Content.Load<Texture2D>("door");
            var snail = Content.Load<Texture2D>("snail");
            var bat = Content.Load<Texture2D>("bat");
            Font = Content.Load<SpriteFont>("font");

            var block = new Texture2D(GraphicsDevice, 1, 1);
            block.SetData(new[] { Color.Red });

            // Initialize Game Systems
            var factory = new LevelObjectFactory(block, spike, cherry, door, snail, bat);
            LevelManager = new LevelManager(factory);
            Hero = new Hero(walk, idle, hurt, death, new KeyBoardReader(), LevelManager)
            {
                Position = new Vector2(100, 100)
            };
            UiManager = new UIManager(Font, Hero);

            // DESIGN PATTERN - Initial State: Load the Start State
            SetGameState(new StartState(Font));
        }

        /* 
         * DESIGN PATTERN - State Switcher:
         * This handles the transition between screens.
         */
        public void SetGameState(IGameState newState)
        {
            _currentState = newState;
        }

        public void RestartGame()
        {
            // DESIGN PATTERN - Reset sequence
            LevelManager.ResetLevel(); // Re-loads map and re-creates all objects via factory
            Hero.Reset(new Vector2(100, 100));
            SetGameState(new PlayingState(this));
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            // SOLID - Open/Closed Principle:
            // Game1 doesn't care WHAT the screen is doing, it just asks it to Update.
            _currentState.Update(this, gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(transformMatrix: _scaleMatrix);

            // SOLID - Dependency Injection: Passing 'this' to satisfy the IGameState.Draw contract.
            _currentState.Draw(this, _spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
