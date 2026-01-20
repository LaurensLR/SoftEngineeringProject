using CherryCollector.Core;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CherryCollector.Systems
{
    /* 
     * SOLID - Single Responsibility: 
     * Handles the math and logic of intersecting rectangles.
     */
    public class CollisionManager
    {
        public void CheckCollisions(IGameObject source, List<IGameObject> targets)
        {
            foreach (var target in targets)
            {
                // Optimization: Don't check an object against itself
                if (source == target) continue;

                /* 
                 * REFACTORING - Intersection Calculation:
                 * Instead of just checking bool Intersects(), we calculate the actual 
                 * collision rectangle. This is often more robust for debugging and 
                 * future features where we might want the 'depth' of collision.
                 */
                Rectangle collision = Rectangle.Intersect(source.Bounds, target.Bounds);

                if (!collision.IsEmpty)
                {
                    /* 
                     * DESIGN PATTERN - Observer/Event Dispatch:
                     * We notify both objects so they can handle the collision 
                     * according to their own internal state (Dead, hurt, etc).
                     */
                    target.OnCollision(source);
                    source.OnCollision(target);
                }
            }
        }
    }
}