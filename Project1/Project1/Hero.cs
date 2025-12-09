using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project1.Input;
using System.Collections.Generic;

namespace Project1
{
    public class Hero : ICollidable, IMovable
    {
        // ✅ IMovable properties
        public Vector2 Position { get; set; }
        public Vector2 Speed { get; set; }
        public IInputReader InputReader { get; set; }

        // ✅ Collision properties
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        public CollisionType CollisionType => CollisionType.Hero;

        // Private fields
        private AnimationManager _animationManager;
        private MovementManager _movementManager;
        private JumpManager _jumpManager;

        private int Width => _animationManager.CurrentAnimation.CurrentFrame.SourceRectangle.Width;
        private int Height => _animationManager.CurrentAnimation.CurrentFrame.SourceRectangle.Height;

        // Constructor
        public Hero(Texture2D walkTexture, Texture2D idleTexture, Texture2D hurtTexture, Texture2D deathTexture,
                    IInputReader inputReader, float groundLevel)
        {
            InputReader = inputReader;

            var idle = new IdleAnimation(idleTexture);
            var walk = new WalkAnimation(walkTexture);
            var hurt = new HurtAnimation(hurtTexture);
            var death = new DeathAnimation(deathTexture);

            _animationManager = new AnimationManager(idle, walk, hurt, death);
            _movementManager = new MovementManager();
            _jumpManager = new JumpManager(groundLevel);

            Position = new Vector2(100, groundLevel);
            Speed = new Vector2(1, 0);
        }

        public void Update(GameTime gameTime, List<ICollidable> worldObjects)
        {
            Vector2 input = InputReader.ReadInput();

            // Horizontal movement
            float moveX = input.X * Speed.X;

            // Future horizontal rectangle
            Rectangle futureHorizontal = new Rectangle(
                (int)(Position.X + moveX),
                (int)Position.Y,
                Width,
                Height
            );

            foreach (var obj in worldObjects)
            {
                if (obj == this) continue;
                if (obj.CollisionType != CollisionType.Block) continue;

                if (futureHorizontal.Intersects(obj.Bounds))
                {
                    // Clamp moveX to edge of block
                    if (moveX > 0) // moving right
                        moveX = obj.Bounds.Left - Width - Position.X;
                    else if (moveX < 0) // moving left
                        moveX = obj.Bounds.Right - Position.X;
                }
            }

            // Apply horizontal movement
            Position = new Vector2(Position.X + moveX, Position.Y);

            // Vertical movement
            // Apply gravity & update vertical position
            _jumpManager.Update(this);

            float moveY = _jumpManager.VelocityY;

            // Future vertical rectangle
            Rectangle futureVertical = new Rectangle(
                (int)Position.X,
                (int)(Position.Y + moveY),
                Width,
                Height
            );

            foreach (var obj in worldObjects)
            {
                if (obj == this) continue;
                if (obj.CollisionType != CollisionType.Block) continue;

                if (futureVertical.Intersects(obj.Bounds))
                {
                    if (moveY > 0) // falling down, landing on top
                    {
                        Position = new Vector2(Position.X, obj.Bounds.Top - Height);
                        _jumpManager.Land();
                    }
                    else if (moveY < 0) // jumping up, hit ceiling
                    {
                        Position = new Vector2(Position.X, obj.Bounds.Bottom);
                        _jumpManager.CancelJump();
                    }

                    // Stop vertical movement
                    _jumpManager.VelocityY = 0;
                    moveY = 0;
                }
            }

            // If no collision, move vertically
            Position = new Vector2(Position.X, Position.Y + moveY);

            // jump input
            if (input.Y > 0)
                _jumpManager.Jump();

            // update animation
            _animationManager.Update(new Vector2(input.X, 0), gameTime);

            // Check spikes
            foreach (var obj in worldObjects)
            {
                if (obj == this) continue;
                if (obj.CollisionType == CollisionType.Spike && Bounds.Intersects(obj.Bounds))
                {
                    OnCollision(obj);
                }
            }
        }


        private void ResolveHorizontalCollisions(List<ICollidable> worldObjects)
        {
            Vector2 input = InputReader.ReadInput();
            float moveX = input.X * Speed.X;

            // Build a rectangle where the hero *would be* if we moved horizontally
            Rectangle futureRect = new Rectangle(
                (int)(Position.X + moveX),
                (int)Position.Y,
                Width,
                Height
            );

            // Check collisions with blocks
            foreach (var obj in worldObjects)
            {
                if (obj == this) continue;
                if (obj.CollisionType != CollisionType.Block) continue;

                if (futureRect.Intersects(obj.Bounds))
                {
                    if (moveX > 0) // Moving right
                        moveX = obj.Bounds.Left - Width - Position.X;
                    else if (moveX < 0) // Moving left
                        moveX = obj.Bounds.Right - Position.X;
                }
            }

            // Apply the horizontal movement
            Position = new Vector2(Position.X + moveX, Position.Y);
        }

        private void ResolveVerticalCollisions(List<ICollidable> worldObjects)
        {
            _jumpManager.Update(this); // Applies gravity

            Rectangle futureVertical = new Rectangle(
                (int)Position.X,
                (int)(Position.Y + _jumpManager.VelocityY), // expose velocityY as public getter
                Width,
                Height
            );

            foreach (var obj in worldObjects)
            {
                if (obj == this) continue;
                if (obj.CollisionType != CollisionType.Block) continue;

                if (futureVertical.Intersects(obj.Bounds))
                {
                    if (_jumpManager.VelocityY > 0) // Falling down
                    {
                        Position = new Vector2(Position.X, obj.Bounds.Top - Height);
                        _jumpManager.Land();
                    }
                    else if (_jumpManager.VelocityY < 0) // Jumping up
                    {
                        Position = new Vector2(Position.X, obj.Bounds.Bottom);
                        _jumpManager.CancelJump();
                    }

                    _jumpManager.VelocityY = 0;
                }
            }

            // Apply vertical movement if no collision
            Position = new Vector2(Position.X, Position.Y + _jumpManager.VelocityY);

        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            var effect = _animationManager.FacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(_animationManager.CurrentAnimation.Texture, Position,
                             _animationManager.CurrentAnimation.CurrentFrame.SourceRectangle,
                             Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
        }

        // OnCollision
        public void OnCollision(ICollidable other)
        {
            if (other.CollisionType == CollisionType.Spike)
            {
                // TODO: Hurt or death animation
            }
        }
    }
}
