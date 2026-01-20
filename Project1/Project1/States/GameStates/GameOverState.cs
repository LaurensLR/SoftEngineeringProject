using CherryCollector;
using CherryCollector.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CherryCollector.States.GameStates
{
    /// <summary>
    ///           GameOverState CLASS - DEATH SCREEN
    ///   PURPOSE:     
    ///   Displayed when the player loses all lives. Offers options to retry the     
    ///   current level or rage quit the game entirely.
    ///   STATE TRANSITIONS:
    ///     GameOverState → [RETRY clicked]→ PlayingState (same level)
    ///     GameOverState → [QUIT clicked]   → Exit game    
    ///   DESIGN PATTERNS APPLIED:
    ///   [STATE PATTERN - Concrete State]    
    ///   Encapsulates death screen behavior. Game1 simply calls Update/Draw
    ///   without knowing this is a "game over" situation.    
    ///   SOLID PRINCIPLES APPLIED:       
    ///   [S] Single Responsibility Principle (SRP):    
    ///       This class ONLY handles:      
    ///       - Displaying the game over screen  
    ///       - Processing retry/quit button clicks    
    ///   KEY DIFFERENCE FROM ResetGame():
    ///   RestartLevel() keeps the current level index - player doesn't lose
    ///   progress to earlier levels. Only the hero state resets.     
    /// </summary>
    public class GameOverState : IGameState
    {
        // UI COMPONENTS
        private readonly Button _retryButton;
        private readonly Button _rqButton;
        private readonly SpriteFont _font;

        /// <summary>
        /// Creates the game over screen with retry and quit buttons.
        /// </summary>
        public GameOverState(SpriteFont font)
        {
            _font = font;
            _retryButton = new Button(new Rectangle(325, 220, 150, 40), "RETRY", font, Color.White);
            _rqButton = new Button(new Rectangle(325, 280, 150, 40), "RAGE QUIT", font, Color.White);
        }

        /// <summary>
        /// Handles button clicks on the game over screen.
        /// </summary>
        public void Update(Game1 game, GameTime gameTime)
        {
            // RETRY: Restart current level only (keeps progress)
            if (_retryButton.IsClicked(game.ScaleMatrix))
            {
                game.RestartLevel();
            }

            // QUIT: Close the game
            if (_rqButton.IsClicked(game.ScaleMatrix))
            {
                game.Exit();
            }
        }

        /// <summary>
        /// Draws the game over screen with title and buttons.
        /// </summary>
        public void Draw(Game1 game, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, "GAME OVER", new Vector2(345, 180), Color.Red);
            _retryButton.Draw(spriteBatch);
            _rqButton.Draw(spriteBatch);
        }
    }
}
