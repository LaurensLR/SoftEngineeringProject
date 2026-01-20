using CherryCollector.Entities.World;
using CherryCollector.Levels;
using CherryCollector.States.GameStates;
using CherryCollector.Systems;
using CherryCollector.Systems.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CherryCollector
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
        public Matrix ScaleMatrix { get; private set; }
        private const int VirtualWidth = 800;
        private const int VirtualHeight = 480;
        private CollisionManager _collisionManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Window.Title = "Cherry dungeons";

            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = false;
            _graphics.HardwareModeSwitch = false;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            float scaleX = (float)GraphicsDevice.Viewport.Width / VirtualWidth;
            float scaleY = (float)GraphicsDevice.Viewport.Height / VirtualHeight;
            float finalScale = MathHelper.Min(scaleX, scaleY);

            // Calculate offset to center the view
            float posX = (GraphicsDevice.Viewport.Width - VirtualWidth * finalScale) / 2;
            float posY = (GraphicsDevice.Viewport.Height - VirtualHeight * finalScale) / 2;

            ScaleMatrix = Matrix.CreateScale(finalScale, finalScale, 1f) *
                          Matrix.CreateTranslation(posX, posY, 0f);

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
            var tile = Content.Load<Texture2D>("tile");
            var snail = Content.Load<Texture2D>("snail");
            var bat = Content.Load<Texture2D>("bat");
            Font = Content.Load<SpriteFont>("font");

            // Initialize Game Systems
            var factory = new LevelObjectFactory(tile, spike, cherry, door, snail, bat);
            LevelManager = new LevelManager(factory);
            _collisionManager = new CollisionManager();
            IInputReader input = new KeyBoardReader();
            Hero = new Hero(walk, idle, hurt, death, input, LevelManager, _collisionManager)
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

        /* 
         * DESIGN PATTERN - Strategy:
         * Differentiates between restarting the current challenge (Level) 
         * vs resetting the entire application state (Game).
         */

        // Call this when pressing "Start" from the main menu or winning the whole game
        public void ResetGame()
        {
            LevelManager.ResetLevel(true); // Loops back to Level 0
            Hero.Reset(new Vector2(100, 100));
            SetGameState(new PlayingState());
        }

        // Call this from the Game Over screen
        public void RestartLevel()
        {
            LevelManager.ResetLevel(false); // Keeps current level index
            Hero.Reset(new Vector2(100, 100)); // Resets hero state
            SetGameState(new PlayingState());
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

            _spriteBatch.Begin(transformMatrix: ScaleMatrix);

            // SOLID - Dependency Injection: Passing 'this' to satisfy the IGameState.Draw contract.
            _currentState.Draw(this, _spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}