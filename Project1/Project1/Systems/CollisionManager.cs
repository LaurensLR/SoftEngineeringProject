using CherryCollector.Core;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CherryCollector.Systems
{
    /// <summary>
    ///       CollisionManager CLASS - COLLISION DETECTION SYSTEM   
    ///   PURPOSE:     
    ///   Centralized system for detecting overlaps between game objects.      
    ///   Notifies both objects when a collision occurs so they can respond.    
    ///   DESIGN PATTERNS APPLIED:
    ///   [OBSERVER PATTERN (Event Dispatch)]    
    ///   CollisionManager notifies BOTH objects of a collision.    
    ///   It doesn't decide what happens - objects handle their own responses.
    ///   This is a form of event dispatch/notification. 
    ///   [MEDIATOR PATTERN]   
    ///   CollisionManager mediates between objects that might collide.
    ///   Objects don't check each other directly - the manager coordinates.       
    ///   SOLID PRINCIPLES APPLIED:
    ///   [S] Single Responsibility Principle (SRP):   
    ///       CollisionManager ONLY detects collisions and dispatches events.    
    ///       It doesn't:
    ///         • Calculate damage (Hero's job)    
    ///      • Mark items collected (Cherry's job)
    ///        • Resolve physics overlap (PhysicsManager's job)        
    /// [O] Open/Closed Principle (OCP):     
    ///   New collision responses can be added by:  
    ///         • Adding new CollisionTypes to the enum    
    ///        • Implementing OnCollision() in new game objects    
    ///       CollisionManager doesn't need modification. 
    ///   [D] Dependency Inversion Principle (DIP):  
    ///       CollisionManager works with IGameObject interface.
    ///       It doesn't know about Hero, Cherry, or Spike specifically. 
    /// </summary>
    public class CollisionManager
    {
        public void CheckCollisions(IGameObject source, List<IGameObject> targets)
        {
            foreach (var target in targets)
            {
                // Optimization: Don't check an object against itself
                if (source == target) continue;


                Rectangle collision = Rectangle.Intersect(source.Bounds, target.Bounds);

                if (!collision.IsEmpty)
                {

                    target.OnCollision(source);
                    source.OnCollision(target);
                }
            }
        }
    }
}