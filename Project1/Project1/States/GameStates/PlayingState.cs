using CherryCollector;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CherryCollector.States.GameStates;

namespace CherryCollector.States.GameStates
{
    /// <summary>
    ///   PlayingState CLASS - ACTIVE GAMEPLAY SCREEN     
    /// PURPOSE:      
    ///   The main gameplay state where the player controls the Hero, collects 
    ///   cherries, avoids hazards, and tries to reach the door to complete levels.  
    ///   STATE TRANSITIONS:
    ///     PlayingState → [Hero dies]     → GameOverState    
    ///     PlayingState → [Level complete]  → Stay (next level) OR WinState   
    ///   DESIGN PATTERNS APPLIED:      
    ///   [STATE PATTERN - Concrete State]        
    ///   This state encapsulates ALL gameplay logic. Game1 doesn't know about
    ///   collision checking, level progression, or death handling - PlayingState   
    ///   handles it all.       
    ///   [MEDIATOR PATTERN]   
    ///   PlayingState acts as a mediator between systems:     
    ///     - Doesn't check door.IsPlayerInside directly      
    ///     - Asks LevelManager.CheckLevelCompletion() instead    
    ///   This decouples PlayingState from internal level implementation.    
    ///   SOLID PRINCIPLES APPLIED:      
    ///   [S] Single Responsibility Principle (SRP):
    ///       PlayingState coordinates gameplay but DELEGATES actual work:   
    ///       - Hero handles its own physics and animation    
    ///       - LevelManager handles level completion logic       
    ///       - UIManager handles HUD rendering      
    ///   [O] Open/Closed Principle (OCP):       
    ///       New gameplay features can be added via the managers without    
    ///  changing PlayingState's structure.     
    /// </summary>
    public class PlayingState : IGameState
    {
        /// <summary>
        /// Default constructor - no special initialization needed.
        /// </summary>
        public PlayingState() { }

        /// <summary>
        /// Main gameplay update loop. Coordinates all game systems.
        /// </summary>
        public void Update(Game1 game, GameTime gameTime)
        {
            // Update all game systems
            game.LevelManager.Update(gameTime);  // Enemies, collectibles
            game.Hero.Update(gameTime);          // Player physics, state
            game.UiManager.Update(gameTime);       // HUD messages

            // DEATH CHECK: Wait for death animation to finish before transitioning
            if (game.Hero.Lives <= 0)
            {
                if (game.Hero.AnimationManager.CurrentAnimation.IsFinished)
                {
                    game.SetGameState(new GameOverState(game.Font));
                }
                return; // Don't check win conditions if dead
            }

            // WIN CHECK: SRP/MEDIATOR - Ask LevelManager, don't inspect door directly
            if (game.LevelManager.CheckLevelCompletion())
            {
                if (game.LevelManager.HasMoreLevels())
                {
                    // Progress to next level
                    game.LevelManager.NextLevel();
                    game.Hero.ResetHero(new Vector2(0, 450));
                }
                else
                {
                    // All levels complete - victory!
                    game.SetGameState(new WinState(game.Font));
                }
            }
            // LOCKED DOOR: Player at door but hasn't collected all cherries
            else if (game.LevelManager.IsPlayerAtLockedDoor())
            {
                game.UiManager.DisplayMessage("COLLECT ALL CHERRIES TO EXIT!", 1.5f);
            }
        }

        /// <summary>
        /// Renders all gameplay elements: level, hero, and HUD.
        /// </summary>
        public void Draw(Game1 game, SpriteBatch spriteBatch)
        {
            game.LevelManager.Draw(spriteBatch);  // Tiles, enemies, items
            game.Hero.Draw(spriteBatch);        // Player character
            game.UiManager.Draw(spriteBatch);      // Lives counter, messages
        }
    }
}
