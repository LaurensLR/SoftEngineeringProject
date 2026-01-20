using CherryCollector.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CherryCollector.States.GameStates
{
    /// <summary>
    ///      StartState CLASS - MAIN MENU SCREEN      
    ///   PURPOSE:
    ///   Represents the main menu state of the game. Displays the title and      
    ///   provides buttons to start the game, toggle difficulty, or quit.   
    ///   STATE TRANSITIONS:     
    ///     StartState → [START clicked] → PlayingState    
    ///     StartState → [QUIT clicked]  → Exit game       
    ///   DESIGN PATTERNS APPLIED:       
    ///   [STATE PATTERN - Concrete State]       
    ///   This is one of the concrete implementations of IGameState.   
    ///   It fully encapsulates menu behavior, keeping Game1 clean.         
    ///   [DEBOUNCE PATTERN]      
    ///   _inputDelay prevents accidental double-clicks when transitioning:     
    ///     - On state entry: 0.5s delay before accepting input     
    ///     - After toggle: 0.2s delay prevents rapid-fire toggling     
    ///   SOLID PRINCIPLES APPLIED:   
    ///   [S] Single Responsibility Principle (SRP):     
    ///       This class ONLY handles main menu logic:       
    ///       - Drawing the menu     
    ///       - Processing menu button clicks     
    ///       - Transitioning to PlayingState  
    ///   HARDCORE MODE:   
    ///   Toggles Hero.MaxLives between 3 (normal) and 1 (hardcore). 
    ///   When MaxLives is 1, one hit means instant game over!    
    /// </summary>
    public class StartState : IGameState
    {
        // UI COMPONENTS - Buttons for user interaction
        private readonly Button _startButton;
        private readonly Button _hardcoreButton;
        private readonly Button _quitButton;
        private readonly SpriteFont _font;

        // DEBOUNCE - Prevents accidental clicks during state transitions
        private float _inputDelay = 0.5f;

        /// <summary>
        /// Creates the menu state with all UI buttons.
        /// </summary>
        public StartState(SpriteFont font)
        {
            _font = font;

            // Create buttons at specific screen positions (800x480 virtual coordinates)
            _startButton = new Button(new Rectangle(325, 220, 150, 40), "START", font, Color.White);
            _hardcoreButton = new Button(new Rectangle(300, 280, 200, 40), "HARDCORE: OFF", font, Color.White);
            _quitButton = new Button(new Rectangle(325, 340, 150, 40), "QUIT", font, Color.White);
        }

        /// <summary>
        /// Updates menu state: handles button clicks with debounce protection.
        /// </summary>
        public void Update(Game1 game, GameTime gameTime)
        {
            // DEBOUNCE: Wait before accepting input
            if (_inputDelay > 0)
            {
                _inputDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                return;
            }

            // Sync button text with actual game state
            _hardcoreButton.Text = (game.Hero.MaxLives == 1) ? "HARDCORE: ON" : "HARDCORE: OFF";

            // START: Begin new game
            if (_startButton.IsClicked(game.ScaleMatrix))
            {
                game.ResetGame();
            }

            // HARDCORE: Toggle difficulty
            if (_hardcoreButton.IsClicked(game.ScaleMatrix))
            {
                game.Hero.MaxLives = (game.Hero.MaxLives == 3) ? 1 : 3;
                _inputDelay = 0.2f; // Prevent rapid toggling
            }

            // QUIT: Exit application
            if (_quitButton.IsClicked(game.ScaleMatrix))
            {
                game.Exit();
            }
        }

        /// <summary>
        /// Draws the menu: title and all buttons.
        /// </summary>
        public void Draw(Game1 game, SpriteBatch spriteBatch)
        {
            // Draw centered title
            string title = "Cherry dungeons";
            Vector2 titleSize = _font.MeasureString(title);
            Vector2 titlePosition = new Vector2(400 - (titleSize.X / 2), 100);
            spriteBatch.DrawString(_font, title, titlePosition, Color.Red);

            // Draw all buttons
            _startButton.Draw(spriteBatch);
            _hardcoreButton.Draw(spriteBatch);
            _quitButton.Draw(spriteBatch);
        }
    }
}
