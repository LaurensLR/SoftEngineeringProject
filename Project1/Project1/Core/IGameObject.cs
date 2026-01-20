namespace CherryCollector.Core
{
    /// <summary>
    ///           IGameObject INTERFACE       
    ///   PURPOSE:             
    ///   The master interface that combines all essential game object behaviors.    
    ///   Any entity in the game world (Hero, enemies, tiles, items) implements
    ///   this interface to participate in the game loop.         
    ///   HOW IT WORKS:       
    ///   IGameObject inherits from three specialized interfaces
    ///   This means any IGameObject can:        
    ///     ✓ Be checked for collisions (has Bounds and CollisionType)       
    ///     ✓ Update its state each frame (has Update method)    
    ///     ✓ Render itself to the screen (has Draw method)        
    ///   DESIGN PATTERN - Composite Pattern:              
    ///   IGameObject composes multiple smaller interfaces into one unified       
    ///   contract. This is the "Composite" approach to interface design:   
    ///     - Each sub-interface (ICollidable, IUpdatable, IDrawable) is         
    ///     a focused "leaf" with a single responsibility       
    ///     - IGameObject is the "composite" that bundles them together    
    ///     - Code can work with the composite OR individual leaves    
    ///   Example: LevelManager stores List<IGameObject>
    ///          CollisionManager only cares about ICollidable aspects
    ///       The Draw loop only cares about IDrawable aspects       
    ///   SOLID PRINCIPLES APPLIED:             
    ///   [I] Interface Segregation Principle (ISP):       
    ///       - Instead of one fat interface with 10+ methods, we have 3 focused
    /// interfaces. Classes implement what they need.     
    ///   [L] Liskov Substitution Principle (LSP):     
    ///       - Any IGameObject can be used wherever ICollidable, IUpdatable,   
    ///         or IDrawable is expected    
    ///       - A Tile can substitute for any IGameObject in a collection    
    ///   [O] Open/Closed Principle (OCP):    
    /// - New game objects can be added by implementing IGameObject       
    ///       - No need to modify existing code (LevelManager, CollisionManager)   
    ///   IMPLEMENTED BY: Hero, Tile, Spike, Cherry, Door, Enemy (base), Snail, Bat   
    /// </summary>
    public interface IGameObject : ICollidable, IUpdatable, IDrawable
    {
        // All methods are inherited from the composed interfaces:
        // From ICollidable: Bounds, CollisionType, OnCollision()
        // From IUpdatable:  Update()
        // From IDrawable:   Draw()
    }
}
