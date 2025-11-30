using Microsoft.Xna.Framework;
using System.Collections.Generic;

internal class CollisionManager
{
    public Rectangle GetBoundingBox(Vector2 position, int width, int height)
    {
        return new Rectangle((int)position.X, (int)position.Y, width, height);
    }

    public bool CheckCollision(Rectangle entityRect, List<Rectangle> obstacles)
    {
        foreach (var obstacle in obstacles)
        {
            if (entityRect.Intersects(obstacle))
            {
                return true;
            }
        }
        return false;
    }
}