using Microsoft.Xna.Framework;

namespace CherryCollector.Core
{
    /// <summary>
    ///              IUpdatable INTERFACE             
    ///   PURPOSE:          
    ///   Defines the contract for any object that has logic to execute each frame.  
    ///   This is the "heartbeat" interface - anything that needs to think, move,    
    ///   animate, or react over time implements this.         
    ///   HOW IT WORKS:            
    ///   - Update() is called once per frame by the game loop      
    ///   - GameTime provides delta time for frame-independent calculations    
    ///   - Objects use this to: animate sprites, apply physics, check input,   
    ///     update AI behavior, count timers, etc.           
    ///   SOLID PRINCIPLES APPLIED:    
    ///   [I] Interface Segregation Principle (ISP):        
    ///     - Separated from IDrawable and ICollidable for maximum flexibility      
    ///       - A static decoration (like a background image) wouldn't need this      
    ///       - Keeps interfaces focused: "Does it update? Then implement IUpdatable" 
    ///   [S] Single Responsibility Principle (SRP):             
    ///       - This interface has ONE job: define the update contract  
    ///       - Each implementing class decides WHAT to update        
    ///   WHY SPLIT INTERFACES?  
    ///   Imagine a "Tile" (floor block). It needs to:              
    ///     ✓ Be drawn (IDrawable)   
    ///     ✓ Block the player (ICollidable)        
    ///     ✗ Update logic? No - it just sits there!        
    ///   With ISP, Tile can implement IUpdatable with an empty body, satisfying      
    ///   the interface without complex unnecessary logic.               
    ///   IMPLEMENTED BY: Hero, Tile, Spike, Cherry, Door, Snail, Bat             
    /// </summary>
    public interface IUpdatable
    {
        /// <summary>
        /// Executes frame-by-frame logic for this object.
        /// </summary>
        /// <param name="gameTime">Provides timing values for frame-independent calculations</param>
        void Update(GameTime gameTime);
    }
}
