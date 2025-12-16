using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Project1
{
    public class MovementManager
    {
        public void MoveHorizontally(IMovable movable, List<ICollidable> worldObjects)
        {
            Vector2 input = movable.InputReader.ReadInput();
            float moveX = input.X * movable.Speed.X;

            Rectangle futureRect = new Rectangle(
                (int)(movable.Position.X + moveX),
                (int)movable.Position.Y,
                movable.Width,
                movable.Height
            );

            foreach (var obj in worldObjects)
            {
                if (obj.CollisionType != CollisionType.Block)
                    continue;

                if (futureRect.Intersects(obj.Bounds))
                {
                    // Clamp movement to block edge
                    if (moveX > 0) // moving right
                        moveX = obj.Bounds.Left - movable.Width - movable.Position.X;
                    else if (moveX < 0) // moving left
                        moveX = obj.Bounds.Right - movable.Position.X;
                }
            }

            movable.Position = new Vector2(movable.Position.X + moveX, movable.Position.Y);
        }

        public void MoveVertically(IMovable movable, JumpManager jumpManager, List<ICollidable> worldObjects)
        {
            // Apply vertical movement (gravity/jump)
            jumpManager.Update(movable);
            float moveY = jumpManager.VelocityY;

            Rectangle futureRect = new Rectangle(
                (int)movable.Position.X,
                (int)(movable.Position.Y + moveY),
                movable.Width,
                movable.Height
            );

            foreach (var obj in worldObjects)
            {
                if (obj.CollisionType != CollisionType.Block)
                    continue;

                if (futureRect.Intersects(obj.Bounds))
                {
                    if (moveY > 0) // falling down
                    {
                        movable.Position = new Vector2(movable.Position.X, obj.Bounds.Top - movable.Height);
                        jumpManager.Land();
                    }
                    else if (moveY < 0) // jumping up
                    {
                        movable.Position = new Vector2(movable.Position.X, obj.Bounds.Bottom);
                        jumpManager.CancelJump();
                    }

                    jumpManager.VelocityY = 0;
                    moveY = 0;
                }
            }

            // If no collision, apply vertical movement
            movable.Position = new Vector2(movable.Position.X, movable.Position.Y + moveY);
        }
    }
}
