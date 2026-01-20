using CherryCollector;
using CherryCollector.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CherryCollector.States.GameStates
{
    /// <summary>
    ///          WinState CLASS - VICTORY SCREEN              
    ///   PURPOSE:      
    ///   Displays the victory screen when the player completes all levels.           
    ///   Provides navigation options to return to menu or quit the game. 
    ///   DESIGN PATTERNS APPLIED:              
    ///   [STATE PATTERN - Concrete State]  
    ///   WinState is one of several IGameState implementations that Game1 can        
    ///   switch between. The game doesn't need complex if-else chains to handle    
    ///   different screens - it just delegates to the current state.      
    ///   STATE TRANSITIONS FROM WinState:        
    ///     WinState ──► StartState (via MENU button)     
    ///     WinState ──► [Application Exit] (via QUIT button)          
    ///   SOLID PRINCIPLES APPLIED:        
    ///   [S] Single Responsibility Principle (SRP):      
    ///       WinState ONLY handles victory screen display and navigation.            
    ///       It doesn't calculate scores, save progress, or manage game logic.       
    ///   [O] Open/Closed Principle (OCP):       
    ///       New victory features (animations, stats) can be added without           
    ///       modifying other game states or the Game1 class.         
    ///   [D] Dependency Inversion Principle (DIP):       
    ///       Game1 depends on IGameState abstraction, not concrete WinState.         
    ///       This allows easy addition of new states (credits, high scores, etc.).   
    /// </summary>
    public class WinState : IGameState
    {
        private readonly Button _menuButton;
        private readonly Button _quitButton;
        private readonly SpriteFont _font;

        public WinState(SpriteFont font)
        {
            _font = font;
            _menuButton = new Button(new Rectangle(325, 230, 150, 40), "MENU", font, Color.Green);
            _quitButton = new Button(new Rectangle(325, 280, 150, 40), "QUIT", font, Color.White);
        }

        public void Update(Game1 game, GameTime gameTime)
        {
            if (_menuButton.IsClicked(game.ScaleMatrix))
            {
                // Going back to the menu implies a full reset next time we play.
                game.SetGameState(new StartState(game.Font));
            }

            if (_quitButton.IsClicked(game.ScaleMatrix))
            {
                game.Exit();
            }
        }

        public void Draw(Game1 game, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, "YOU WIN!", new Vector2(355, 150), Color.Gold);
            _menuButton.Draw(spriteBatch);
            _quitButton.Draw(spriteBatch);
        }
    }
}
