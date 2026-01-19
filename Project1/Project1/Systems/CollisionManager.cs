using CherryCollector.Core;
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

                if (source.Bounds.Intersects(target.Bounds))
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