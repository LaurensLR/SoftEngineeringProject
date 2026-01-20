using CherryCollector.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace CherryCollector.Systems
{
    /// <summary>
    ///      PhysicsManager CLASS - PHYSICS SIMULATION SYSTEM 
    ///   PURPOSE:    
    ///   Centralized physics system that handles gravity, velocity, jumping, and    
    ///   collision resolution for movable entities (Hero). 
    ///   DESIGN PATTERNS APPLIED:    
    ///   [COMPONENT PATTERN / MANAGER PATTERN]       
    ///   PhysicsManager encapsulates all physics logic in one place.     
    ///   Hero doesn't calculate physics - it delegates to this manager.   
    ///   This makes physics:  
    ///     • Reusable (other entities could use it)    
    ///     • Testable (can unit test physics in isolation)  
    ///    • Maintainable (physics changes don't touch Hero code)   
    ///   [SEPARATION OF CONCERNS]    
    ///   Physics (movement, gravity, collision) is separated from:   
    ///    • Input (handled by IInputReader)
    ///     • Animation (handled by AnimationManager) 
    ///     • Game logic (handled by Hero states)
    ///   SOLID PRINCIPLES APPLIED:     
    ///   [S] Single Responsibility Principle (SRP):  
    ///       PhysicsManager ONLY handles physics calculations.    
    ///       It doesn't read input, play animations, or track lives.    
    ///   [O] Open/Closed Principle (OCP):
    ///       New physics features (double jump, wall slide) can be added without    
    ///       modifying existing Hero or NormalState code.
    ///   [D] Dependency Inversion Principle (DIP):   
    ///       PhysicsManager works with IMovable interface.
    ///       It doesn't depend on Hero specifically - could work with any movable.  
    ///   PHYSICS CONSTANTS:       
    ///     • Gravity: 1500 pixels/sec² (downward acceleration)   
    ///     • JumpStrength: -450 pixels/sec (initial upward velocity)    
    ///     • BounceStrength: -350 pixels/sec (damage bounce)     
    ///     • MapWidth: 800 pixels (prevents leaving screen)     
    ///     • MaxPenetrationMap: 24 pixels (max overlap correction)  
    /// </summary>
    public class PhysicsManager
    {
        private const int MapWidth = 800;
        private const float SkinWidth = 0.01f;
        private const float MaxPenetrationMap = 24f; // Allow fixing overlap up to 24 pixels (player height is 28)

        // Physics constants
        private float _gravity = 1500f;
        private float _jumpStrength = -450f;
        private float _bounceStrength = -350f;


        public Vector2 Velocity { get; set; }
        public bool IsGrounded { get; private set; }

        public void Jump()
        {
            if (IsGrounded)
            {
                Velocity = new Vector2(Velocity.X, _jumpStrength);
                IsGrounded = false;
            }
        }

        public void CancelJump()
        {
            if (Velocity.Y < 0)
                Velocity = new Vector2(Velocity.X, 0);
        }

        public void ApplyBounce()
        {
            Velocity = new Vector2(Velocity.X, _bounceStrength);
            IsGrounded = false;
        }

        public void Reset()
        {
            Velocity = Vector2.Zero;
            IsGrounded = false;
        }


        public void Update(IMovable entity, List<IGameObject> worldObjects, GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // 1. Calculate desired velocity
            // Gravity
            float newVy = Velocity.Y + _gravity * dt;

            // Input (Horizontal Movement)
            Vector2 input = entity.InputReader.ReadInput();
            float newVx = input.X * entity.Speed.X;

            Velocity = new Vector2(newVx, newVy);

            // 2. Apply Movement with Collision Resolution
            Move(entity, Velocity * dt, worldObjects);
        }

        private void Move(IMovable entity, Vector2 amount, List<IGameObject> worldObjects)
        {
            // Horizontal
            float moveX = CalculateHorizontalAllowance(entity, amount.X, worldObjects);
            entity.Position = new Vector2(entity.Position.X + moveX, entity.Position.Y);

            // Vertical
            float moveY = ResolveVerticalCollision(entity, amount.Y, worldObjects);
            entity.Position = new Vector2(entity.Position.X, entity.Position.Y + moveY);
        }

        private float CalculateHorizontalAllowance(IMovable movable, float desiredMoveX, List<IGameObject> worldObjects)
        {
            if (Math.Abs(desiredMoveX) < SkinWidth) return 0;

            float moveX = desiredMoveX;
            Rectangle futureRect = new Rectangle(
                (int)(movable.Position.X + desiredMoveX),
                (int)movable.Position.Y,
                movable.Width,
                movable.Height
            );

            // Boundary Check
            if (futureRect.Left < 0) moveX = Math.Max(moveX, -movable.Position.X);
            else if (futureRect.Right > MapWidth) moveX = Math.Min(moveX, MapWidth - movable.Position.X - movable.Width);

            foreach (var obj in worldObjects)
            {
                if (obj.CollisionType != CollisionType.Block) continue;
                if (Vector2.Distance(movable.Position, new Vector2(obj.Bounds.X, obj.Bounds.Y)) > 100) continue;
                if (!futureRect.Intersects(obj.Bounds)) continue;

                if (desiredMoveX > 0) // Right
                {
                    float distance = obj.Bounds.Left - (movable.Position.X + movable.Width);
                    if (distance >= -SkinWidth) moveX = Math.Min(moveX, distance);
                }
                else // Left
                {
                    float distance = obj.Bounds.Right - movable.Position.X;
                    if (distance <= SkinWidth) moveX = Math.Max(moveX, distance);
                }
            }
            return moveX;
        }

        private float ResolveVerticalCollision(IMovable movable, float desiredMoveY, List<IGameObject> worldObjects)
        {
            float moveY = desiredMoveY;
            IsGrounded = false; // Assume air until proven grounded

            Rectangle futureRect = new Rectangle(
                (int)movable.Position.X,
                (int)(movable.Position.Y + desiredMoveY),
                movable.Width,
                movable.Height
            );

            foreach (var obj in worldObjects)
            {
                if (obj.CollisionType != CollisionType.Block) continue;
                if (Vector2.Distance(movable.Position, new Vector2(obj.Bounds.X, obj.Bounds.Y)) > 100) continue;
                if (!futureRect.Intersects(obj.Bounds)) continue;

                if (desiredMoveY > 0) // Falling
                {
                    float distance = obj.Bounds.Top - (movable.Position.Y + movable.Height);


                    // This catches the case where gravity pulled us 5-10 pixels deep in one frame.
                    if (distance >= -MaxPenetrationMap)
                    {
                        moveY = Math.Min(moveY, distance);
                        IsGrounded = true;
                        Velocity = new Vector2(Velocity.X, 0); // Stop falling
                    }
                }
                else if (desiredMoveY < 0) // Jumping
                {
                    float distance = obj.Bounds.Bottom - movable.Position.Y;
                    if (distance <= MaxPenetrationMap) // Allow snap down if we hit head deep
                    {
                        moveY = Math.Max(moveY, distance);
                        Velocity = new Vector2(Velocity.X, 0); // Bonk head
                    }
                }
            }
            return moveY;
        }
    }
}