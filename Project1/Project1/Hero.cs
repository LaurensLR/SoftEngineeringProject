using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project1.Input;

namespace Project1
{
    class Hero : IGameObject, IMovable
    {
        private MovementManager _movementManager;
        private AnimationManager _animationManager;
        private JumpManager _jumpManager;

        public Vector2 Position { get; set; }
        public Vector2 Speed { get; set; }
        public IInputReader InputReader { get; set; }

        public Hero(Texture2D walkTexture, Texture2D idleTexture, IInputReader inputReaderIn, float groundLevel)
        {
            InputReader = inputReaderIn;

            var idleAnimation = new IdleAnimation(idleTexture);
            var walkAnimation = new WalkAnimation(walkTexture);

            _animationManager = new AnimationManager(idleAnimation, walkAnimation);
            _movementManager = new MovementManager();
            _jumpManager = new JumpManager(groundLevel);

            Position = new Vector2(10, groundLevel);  // Start at ground level
            Speed = new Vector2(1, 0);
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
            var direction = InputReader.ReadInput();

            // Handle jump input
            if (direction.Y > 0)
            {
                _jumpManager.Jump();
            }

            // Only move horizontally (X direction)
            var horizontalDirection = new Vector2(direction.X, 0);

            // Update horizontal movement
            var tempDirection = horizontalDirection;
            _movementManager.Move(this);

            // Apply gravity and jumping
            _jumpManager.Update(this);

            // Update animation with horizontal direction only
            _animationManager.Update(horizontalDirection, gameTime);
        }
    }
}