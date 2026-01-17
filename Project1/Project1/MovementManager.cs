using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Project1
{
    public class MovementManager
    {
        public void MoveHorizontally(IMovable movable, List<ICollidable> worldObjects)
        {
            Vector2 input = movable.InputReader.ReadInput();
            float desiredMoveX = input.X * movable.Speed.X;
            float moveX = desiredMoveX;

            // Use the desired future rect to test potential collisions
            Rectangle futureRectDesired = new Rectangle(
                (int)(movable.Position.X + desiredMoveX),
                (int)movable.Position.Y,
                movable.Width,
                movable.Height
            );

            foreach (var obj in worldObjects)
            {
                if (obj.CollisionType != CollisionType.Block)
                    continue;

                if (!futureRectDesired.Intersects(obj.Bounds))
                    continue;

                if (desiredMoveX > 0) // moving right
                {
                    // allowed move so that right edge == block left
                    float allowed = obj.Bounds.Left - movable.Width - movable.Position.X;
                    moveX = Math.Min(moveX, allowed);
                }
                else if (desiredMoveX < 0) // moving left
                {
                    // allowed move so that left edge == block right
                    float allowed = obj.Bounds.Right - movable.Position.X;
                    moveX = Math.Max(moveX, allowed);
                }
            }

            movable.Position = new Vector2(movable.Position.X + moveX, movable.Position.Y);
        }

        public void MoveVertically(IMovable movable, JumpManager jumpManager, List<ICollidable> worldObjects)
        {
            // Get intended vertical delta from JumpManager (does not mutate position)
            float desiredMoveY = jumpManager.Update(movable);
            float moveY = desiredMoveY;

            Rectangle futureRectDesired = new Rectangle(
                (int)movable.Position.X,
                (int)(movable.Position.Y + desiredMoveY),
                movable.Width,
                movable.Height
            );

            bool collided = false;
            bool landed = false;
            bool hitCeiling = false;

            foreach (var obj in worldObjects)
            {
                if (obj.CollisionType != CollisionType.Block)
                    continue;

                if (!futureRectDesired.Intersects(obj.Bounds))
                    continue;

                if (desiredMoveY > 0) // falling down
                {
                    float allowed = obj.Bounds.Top - movable.Height - movable.Position.Y;
                    moveY = Math.Min(moveY, allowed);
                    collided = true;
                    landed = true;
                }
                else if (desiredMoveY < 0) // moving up
                {
                    float allowed = obj.Bounds.Bottom - movable.Position.Y;
                    moveY = Math.Max(moveY, allowed);
                    collided = true;
                    hitCeiling = true;
                }
            }

            // Apply collision responses
            if (collided)
            {
                // Apply the clamped move once
                movable.Position = new Vector2(movable.Position.X, movable.Position.Y + moveY);

                if (landed)
                {
                    jumpManager.Land();
                }
                else if (hitCeiling)
                {
                    jumpManager.CancelJump();
                }

                jumpManager.VelocityY = 0;
                return; // movement already applied
            }

            // If no collision, apply vertical movement
            movable.Position = new Vector2(movable.Position.X, movable.Position.Y + moveY);
        }
    }
}
