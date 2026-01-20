using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CherryCollector.States.GameStates
{
    /// <summary>
    ///         IGameState INTERFACE        
    ///   PURPOSE:  
    ///   Defines the contract for game screen states. Each state represents a       
    ///   distinct "mode" of the game with its own update logic and visuals.
    ///   Each concrete state:      
    ///     - Handles its own input (buttons, keys)     
    ///     - Manages its own UI elements (buttons, text)  
    ///     - Can trigger transitions via game.SetGameState(new OtherState())    
    ///   DESIGN PATTERN - STATE PATTERN:         
    ///   The State Pattern allows an object to change its behavior when its  
    ///   internal state changes. The object appears to change its class.    
    ///   WITHOUT State Pattern (BAD):         
    ///     if (gameState == "menu") { /* 100 lines of menu code */ }   
    ///     else if (gameState == "playing") { /* 100 lines of game code */ }    
    ///     else if (gameState == "gameover") { /* 50 lines */ }   
    ///     // Messy, hard to maintain, violates OCP      
    ///   WITH State Pattern (GOOD):    
    ///     _currentState.Update(game, gameTime);  // One line, delegates to state   
    ///     // Each state is its own class with focused responsibility    
    ///   SOLID PRINCIPLES APPLIED:     
    ///   [S] Single Responsibility Principle (SRP):     
    ///       - Each concrete state handles ONE screen's logic     
    ///       - StartState only cares about menu, PlayingState only about gameplay  
    ///   [O] Open/Closed Principle (OCP):     
    ///       - Game1 is closed for modification when adding states        
    ///       - Just create a new class implementing IGameState       
    ///       - Example: Adding PauseState requires ZERO changes to Game1       
    ///   [L] Liskov Substitution Principle (LSP):      
    ///       - Any IGameState can be substituted for another     
    ///  - Game1 treats all states identically through the interface          
    ///   [D] Dependency Inversion Principle (DIP):   
    /// - Game1 depends on IGameState abstraction, not concrete states    
    ///       - States receive Game1 to access needed dependencies      
    ///   IMPLEMENTED BY: StartState, PlayingState, GameOverState, WinState     
    /// </summary>
    public interface IGameState
    {
        /// <summary>
        /// Processes input and updates state logic each frame.
        /// </summary>
        /// <param name="game">Reference to Game1 for accessing Hero, LevelManager, etc.</param>
        /// <param name="gameTime">Timing info for frame-independent updates</param>
        void Update(Game1 game, GameTime gameTime);

        /// <summary>
        /// Renders the state's visuals to the screen.
        /// </summary>
        /// <param name="game">Reference to Game1 for accessing drawable objects</param>
        /// <param name="spriteBatch">MonoGame SpriteBatch for 2D rendering</param>
        void Draw(Game1 game, SpriteBatch spriteBatch);
    }
}