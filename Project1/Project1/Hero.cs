using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project1.Content;
using Project1.Input;
using System.Collections.Generic;

namespace Project1
{
    class Hero : ICollidable, IMovable
    {
        private MovementManager _movementManager;
        private CollisionManager _collisionManager;
        private AnimationManager _animationManager;
        private JumpManager _jumpManager;

        public Vector2 Position { get; set; }
        public Vector2 Speed { get; set; }
        public IInputReader InputReader { get; set; }
        private int _width { get
            {
                return _animationManager.CurrentAnimation.CurrentFrame.SourceRectangle.Width;
            } }
        private int _height
        {
            get
            {
                return _animationManager.CurrentAnimation.CurrentFrame.SourceRectangle.Height;
            }
        }

        public Hero(Texture2D walkTexture, Texture2D idleTexture, Texture2D hurtTexture, Texture2D deathTexture , IInputReader inputReaderIn, float groundLevel)
        {
            InputReader = inputReaderIn;

            var idleAnimation = new IdleAnimation(idleTexture);
            var walkAnimation = new WalkAnimation(walkTexture);
            var hurtAnimation = new HurtAnimation(hurtTexture);
            var deathAnimation = new DeathAnimation(deathTexture);

            _animationManager = new AnimationManager(idleAnimation, walkAnimation, hurtAnimation, deathAnimation);
            _movementManager = new MovementManager();
            _jumpManager = new JumpManager(groundLevel);
            Position = new Vector2(100, groundLevel);
            Speed = new Vector2(1, 0);
            _collisionManager = new CollisionManager();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var spriteEffect = _animationManager.FacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            spriteBatch.Draw(_animationManager.CurrentAnimation.Texture,
                           Position,
                           _animationManager.CurrentAnimation.CurrentFrame.SourceRectangle,
                           Color.White, 0f, Vector2.Zero, 1f,
                           spriteEffect, 0f);
        }

        public void Update(GameTime gameTime)
        {
            Update(gameTime, new List<Rectangle>());
        }

        public void Update(GameTime gameTime, List<Rectangle> obstacles)
        {
            var direction = InputReader.ReadInput();

            // Handle jump input
            if (direction.Y > 0)
            {
                _jumpManager.Jump();
            }

            var horizontalDirection = new Vector2(direction.X, 0);

            // ===== HORIZONTAL MOVEMENT WITH COLLISION =====
            Vector2 oldPosition = Position;
            _movementManager.Move(this);

            Rectangle heroRect = new Rectangle((int)Position.X, (int)Position.Y, _width, _height);
            foreach (var obstacle in obstacles)
            {
                if (heroRect.Intersects(obstacle))
                {
                    if (Position.X < oldPosition.X) // Moving left
                    {
                        Position = new Vector2(obstacle.Right, Position.Y);
                    }
                    else if (Position.X > oldPosition.X) // Moving right
                    {
                        Position = new Vector2(obstacle.Left - _width, Position.Y);
                    }
                }
            }

            // ===== VERTICAL MOVEMENT WITH COLLISION =====
            oldPosition = Position;
            _jumpManager.Update(this);

            heroRect = new Rectangle((int)Position.X, (int)Position.Y, _width, _height);
            foreach (var obstacle in obstacles)
            {
                if (heroRect.Intersects(obstacle))
                {
                    if (Position.Y < oldPosition.Y) // Moving up
                    {
                        Position = new Vector2(Position.X, obstacle.Bottom);
                        _jumpManager.CancelJump();
                    }
                    else if (Position.Y > oldPosition.Y) // Moving down
                    {
                        Position = new Vector2(Position.X, obstacle.Top - _height);
                        _jumpManager.Land();
                    }
                }
            }

            _animationManager.Update(horizontalDirection, gameTime);
        }
    }
}