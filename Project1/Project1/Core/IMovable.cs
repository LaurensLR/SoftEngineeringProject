using CherryCollector.Systems.Input;
using Microsoft.Xna.Framework;

namespace CherryCollector.Core
{
    /// <summary>
    ///    IMovable INTERFACE       
    ///   PURPOSE:         
    ///   Defines the contract for entities that can move through the game world.   
    ///   This interface is used by the PhysicsManager to apply movement, gravity,   
    ///   and collision resolution without knowing the specific entity type.       
    ///   HOW IT WORKS:           
    ///   The PhysicsManager receives any IMovable and can:          
    ///     1. Read current Position and Speed         
    ///     2. Read player input via InputReader    
    ///     3. Calculate collisions using Width and Height       
    ///     4. Write back the new Position after physics calculations    
    ///   This creates a "physics-enabled" contract that the Hero implements.   
    ///   SOLID PRINCIPLES APPLIED:        
    ///   [I] Interface Segregation Principle (ISP):
    ///       - Separate from IGameObject because not all game objects move  
    ///       - Static objects (Tile, Cherry, Door) don't implement this    
    ///   [D] Dependency Inversion Principle (DIP):       
    ///       - PhysicsManager depends on IMovable abstraction    
    ///       - Could easily add other movable entities (NPC, projectiles)       
    ///     without modifying PhysicsManager       
    ///   DESIGN PATTERN - Strategy Pattern (via InputReader):      
    ///   The InputReader property allows different input strategies:   
    ///     - KeyBoardReader: Reads keyboard input     
    ///  - Could add: GamePadReader, AIInputReader, NetworkInputReader  
    ///   The movable entity doesn't care WHERE input comes from.
    ///   IMPLEMENTED BY: Hero     
    /// </summary>
    public interface IMovable
    {
        /// <summary>Current world position (top-left corner)</summary>
        Vector2 Position { get; set; }

        /// <summary>Movement speed multiplier (pixels per second)</summary>
        Vector2 Speed { get; set; }

        /// <summary>The input strategy for reading movement commands</summary>
        IInputReader InputReader { get; set; }

        /// <summary>Entity width for collision calculations</summary>
        int Width { get; }

        /// <summary>Entity height for collision calculations</summary>
        int Height { get; }
    }
}
