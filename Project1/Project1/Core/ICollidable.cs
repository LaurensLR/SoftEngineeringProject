using Microsoft.Xna.Framework;

namespace CherryCollector.Core
{
    /// <summary>
    ///           CollisionType ENUM
    ///   Categorizes game objects for collision handling. 
    ///   When two objects collide, their types determine the response:   
    ///   - Hero + Cherry = Collect cherry           
    ///     - Hero + Spike/Enemy = Take damage          
    /// - Hero + Block = Stop movement               
    ///     - Hero + Door = Check for level completion
    /// </summary>
    public enum CollisionType { Hero, Block, Spike, Cherry, Door, Enemy }

    /// <summary>
    ///      ICollidable INTERFACE          
    ///   PURPOSE:          
    ///   Defines the contract for any object that participates in collision   
    ///   detection. This enables the physics system to treat all collidable          
    ///   objects uniformly without knowing their specific types.       
    ///   HOW IT WORKS:         
    ///   1. Bounds: Returns a Rectangle representing the object's hitbox             
    ///      - Used by CollisionManager to check for intersections   
    ///      - Can be smaller than visual sprite for "forgiving" hit detection   
    ///   2. CollisionType: Identifies what KIND of object this is       
    ///      - Enables type-specific collision responses        
    ///      - Example: Hero only takes damage from Spike/Enemy types            
    ///   3. OnCollision(): Called when this object collides with another         
    ///      - Each object decides how to react to collisions       
    ///      - Example: Cherry marks itself as collected when hit by Hero    
    ///   SOLID PRINCIPLES APPLIED:            
    ///   [I] Interface Segregation Principle (ISP):              
    ///       - Focused solely on collision-related properties and behavior       
    /// - Non-physical objects (like UI elements) don't need this        
    ///   [D] Dependency Inversion Principle (DIP):            
    ///  - CollisionManager depends on ICollidable abstraction, not concrete     
    ///         classes like "Hero" or "Spike"      
    ///       - New collidable objects can be added without changing the manager
    /// DESIGN PATTERN - Double Dispatch:          
    ///   When collision is detected, BOTH objects get notified via OnCollision().    
    ///   This allows each object to respond according to its own rules:  
    ///     - Cherry: "I was hit by Hero, mark myself collected"          
    ///     - Hero: "I hit a Spike, take damage"  
    ///     - Tile: "I don't care about collisions, do nothing"       
    ///   IMPLEMENTED BY: Hero, Tile, Spike, Cherry, Door, Enemy (base), Snail, Bat   
    /// </summary>
    public interface ICollidable
    {
        /// <summary>
        /// The collision hitbox for this object (position + size).
        /// </summary>
        Rectangle Bounds { get; }

        /// <summary>
        /// Identifies the category of this object for collision response logic.
        /// </summary>
        CollisionType CollisionType { get; }

        /// <summary>
        /// Called when this object collides with another IGameObject.
        /// Implement collision response logic here.
        /// </summary>
        /// <param name="other">The other object involved in the collision</param>
        void OnCollision(IGameObject other);
    }
}
