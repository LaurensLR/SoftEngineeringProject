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
    /// <summary>
    ///      Game1 CLASS - MAIN GAME ENGINE     
    ///   PURPOSE:      
    ///   The central hub of the game. Inherits from MonoGame's Game class and       
    ///   orchestrates the entire game lifecycle: initialization, content loading,   
    ///   the game loop (update + draw), and cleanup.    
    ///   KEY RESPONSIBILITIES:      
    ///     1. Creates and manages all core systems (Physics, Collision, UI, Level)  
    ///     2. Delegates update/draw to the current IGameState  
    ///  3. Handles screen scaling for resolution independence
    ///   4. Provides state transition methods (SetGameState, ResetGame, etc.)   
    ///   DESIGN PATTERNS APPLIED:     
    ///   [STATE PATTERN] 
    ///   The game uses IGameState to manage different screens:     
    ///   - StartState    → Main menu        
    ///     - PlayingState  → Active gameplay       
    ///     - GameOverState → Death screen           
    ///    - WinState    → Victory screen 
    ///   The current state is stored in _currentState, and Update()/Draw() simply   
    ///   delegate to it. This eliminates massive if-else chains like:    
    ///     BAD:  if (isPlaying) {...} else if (isMenu) {...} else if...     
    ///     GOOD: _currentState.Update(this, gameTime);           
    ///   [DEPENDENCY INJECTION]
    ///   Game1 creates all dependencies and "injects" them into objects:  
    ///     - Hero receives: InputReader, LevelManager, CollisionManager, Physics   
    ///     - States receive: Game1 reference (to access Hero, LevelManager, etc.)  
    ///   This makes testing easier and reduces coupling.          
    ///   [STRATEGY PATTERN]        
    ///   ResetGame() vs RestartLevel() - two different reset strategies:    
    ///     - ResetGame(): Full reset to level 0 (from menu or after winning)    
    ///     - RestartLevel(): Keep current level, just reset hero (after death)   
    ///   SOLID PRINCIPLES APPLIED:       
    ///   [O] Open/Closed Principle (OCP):       
    ///       Game1 is OPEN for extension (add new states) but CLOSED for  
    ///       modification. Adding a PauseState doesn't require changing Update().  
    ///   [D] Dependency Inversion Principle (DIP):        
    ///       Game1 depends on abstractions (IGameState, IInputReader) not  
    ///       concrete implementations. Easy to swap KeyBoardReader for GamePad.   
    ///   [S] Single Responsibility Principle (SRP):   
    ///     Game1 orchestrates, but delegates actual work to specialized classes: 
    ///       - Physics → PhysicsManager   
    ///       - Collisions → CollisionManager           
    ///       - UI → UIManager   
    ///       - Levels → LevelManager 
    ///   SCREEN SCALING SYSTEM:
    ///   The game is designed for 800x480 (VirtualWidth x VirtualHeight).     
    ///   ScaleMatrix transforms this to any screen resolution while maintaining     
    ///   aspect ratio and centering the view. All game logic uses virtual coords.  
    /// </summary>
    public class Game1 : Game
    {
        // MONOGAME CORE OBJECTS
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // GAME OBJECTS - Public for state access (DI pattern)
        public Hero Hero { get; private set; }
        public LevelManager LevelManager { get; private set; }
        public UIManager UiManager { get; private set; }
        public SpriteFont Font { get; private set; }

        // STATE PATTERN - Current game state (menu, playing, gameover, win)
        private IGameState _currentState;

        // SCREEN SCALING - Resolution independence
        public Matrix ScaleMatrix { get; private set; }
        private const int VirtualWidth = 800;
        private const int VirtualHeight = 480;

        // SYSTEM MANAGERS - Encapsulated physics and collision handling
        private CollisionManager _collisionManager;
        private PhysicsManager _physicsManager;

        /// <summary>
        /// Constructor: Configures graphics settings before the game starts.
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.Title = "Cherry dungeons";
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.HardwareModeSwitch = false;
            _graphics.ApplyChanges();
        }

        /// <summary>
        /// Initialize: Called after constructor. Sets up the scaling matrix
        /// for resolution-independent rendering.
        /// </summary>
        protected override void Initialize()
        {
            // Calculate scale to fit virtual resolution in actual screen
            float scaleX = (float)GraphicsDevice.Viewport.Width / VirtualWidth;
            float scaleY = (float)GraphicsDevice.Viewport.Height / VirtualHeight;
            float finalScale = MathHelper.Min(scaleX, scaleY);

            // Calculate offset to center the view (letterboxing)
            float posX = (GraphicsDevice.Viewport.Width - VirtualWidth * finalScale) / 2;
            float posY = (GraphicsDevice.Viewport.Height - VirtualHeight * finalScale) / 2;

            ScaleMatrix = Matrix.CreateScale(finalScale, finalScale, 1f) *
             Matrix.CreateTranslation(posX, posY, 0f);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent: Loads all game assets and creates game objects.
        /// This is where DEPENDENCY INJECTION happens - objects receive their dependencies.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load all textures from Content pipeline
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

            // Create factory and managers (FACTORY PATTERN for level objects)
            var factory = new LevelObjectFactory(tile, spike, cherry, door, snail, bat);
            LevelManager = new LevelManager(factory);
            _collisionManager = new CollisionManager();
            _physicsManager = new PhysicsManager();

            // Create input strategy (STRATEGY PATTERN - could swap for GamePadReader)
            IInputReader input = new KeyBoardReader();

            // Create hero with all dependencies injected
            Hero = new Hero(walk, idle, hurt, death, input, LevelManager, _collisionManager, _physicsManager)
            {
                Position = new Vector2(0, 450)
            };
            UiManager = new UIManager(Font, Hero);

            // STATE PATTERN: Start with menu screen
            SetGameState(new StartState(Font));
        }

        /// <summary>
        /// STATE PATTERN: Switches the current game state.
        /// This single method controls all screen transitions.
        /// </summary>
        public void SetGameState(IGameState newState)
        {
            _currentState = newState;
        }

        /// <summary>
        /// STRATEGY: Full game reset (from menu or after winning all levels).
        /// Resets to level 0 and starts fresh.
        /// </summary>
        public void ResetGame()
        {
            LevelManager.ResetLevel(true);  // true = go back to level 0
            Hero.Reset(new Vector2(0, 450));
            SetGameState(new PlayingState());
        }

        /// <summary>
        /// STRATEGY: Restart current level only (after death).
        /// Keeps progress but resets hero state.
        /// </summary>
        public void RestartLevel()
        {
            LevelManager.ResetLevel(false); // false = stay on current level
            Hero.Reset(new Vector2(0, 450));
            SetGameState(new PlayingState());
        }

        /// <summary>
        /// Update: Called 60 times per second. Delegates to current state.
        /// OCP in action: Adding new states doesn't require modifying this method.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            // STATE PATTERN: Delegate update to current state
            // Game1 doesn't know or care WHAT the state is doing
            _currentState.Update(this, gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Draw: Called after Update. Renders the game with scaling applied.
        /// Passes 'this' to state for dependency access.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Apply scaling matrix for resolution independence
            _spriteBatch.Begin(transformMatrix: ScaleMatrix);

            // STATE PATTERN: Delegate drawing to current state
            _currentState.Draw(this, _spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}