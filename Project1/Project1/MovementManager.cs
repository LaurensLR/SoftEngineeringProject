using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Project1
{
    /* 
    * SOLID - Single Responsibility Principle (SRP):
    * This class is ONLY responsible for the horizontal and vertical physics math. 
    */
    public class MovementManager
    {
        public void MoveHorizontally(IMovable movable, List<IGameObject> worldObjects, GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 input = movable.InputReader.ReadInput();

            // REFACTORING: Horizontal displacement is now pixels-per-second * deltaTime
            float desiredMoveX = input.X * movable.Speed.X * dt;

            float moveX = CalculateHorizontalAllowance(movable, desiredMoveX, worldObjects);

            movable.Position = new Vector2(movable.Position.X + moveX, movable.Position.Y);
        }

        public void MoveVertically(IMovable movable, JumpManager jumpManager, List<IGameObject> worldObjects, GameTime gameTime)
        {
            // Calculate vertical delta based on seconds passed
            float desiredMoveY = jumpManager.CalculateDeltaY(gameTime);

            bool collided = ResolveVerticalCollision(movable, jumpManager, desiredMoveY, worldObjects, out float moveY);

            movable.Position = new Vector2(movable.Position.X, movable.Position.Y + moveY);

            if (collided)
            {
                jumpManager.VelocityY = 0;
            }
        }

        /*
         * SOLID - Single Responsibility Principle (SRP):
         * These private helper methods handle the "low-level" boundary calculations.
         * This prevents the Move methods from becoming "God Methods" that handle too many 
         * concerns (input, physics state, AND boundary math) in one massive block.
         */

        private float CalculateHorizontalAllowance(IMovable movable, float desiredMoveX, List<IGameObject> worldObjects)
        {
            if (desiredMoveX == 0) return 0;

            float moveX = desiredMoveX;
            Rectangle futureRect = new Rectangle(
                (int)(movable.Position.X + desiredMoveX),
                (int)movable.Position.Y,
                movable.Width,
                movable.Height
            );

            foreach (var obj in worldObjects)
            {
                // We only perform physics resolution against solid Block objects
                if (obj.CollisionType != CollisionType.Block || !futureRect.Intersects(obj.Bounds))
                    continue;

                if (desiredMoveX > 0) // Moving right, clamped to left edge of block
                    moveX = Math.Min(moveX, obj.Bounds.Left - movable.Width - movable.Position.X);
                else // Moving left, clamped to right edge of block
                    moveX = Math.Max(moveX, obj.Bounds.Right - movable.Position.X);
            }

            return moveX;
        }

        private bool ResolveVerticalCollision(IMovable movable, JumpManager jumpManager, float desiredMoveY, List<IGameObject> worldObjects, out float moveY)
        {
            moveY = desiredMoveY;
            bool hitObstacle = false;

            Rectangle futureRect = new Rectangle(
                (int)movable.Position.X,
                (int)(movable.Position.Y + desiredMoveY),
                movable.Width,
                movable.Height
            );

            foreach (var obj in worldObjects)
            {
                if (obj.CollisionType != CollisionType.Block || !futureRect.Intersects(obj.Bounds))
                    continue;

                hitObstacle = true;
                if (desiredMoveY > 0) // Falling down
                {
                    // Clamp to top of block and notify JumpManager to Land
                    moveY = Math.Min(moveY, obj.Bounds.Top - movable.Height - movable.Position.Y);
                    jumpManager.Land();
                }
                else if (desiredMoveY < 0) // Moving up (Jumping)
                {
                    // Clamp to bottom of block and notify JumpManager to stop upward momentum
                    moveY = Math.Max(moveY, obj.Bounds.Bottom - movable.Position.Y);
                    jumpManager.CancelJump();
                }
            }

            return hitObstacle;
        }
    }
}
