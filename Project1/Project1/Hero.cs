using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project1.Input;

namespace Project1
{
    class Hero : IGameObject, IMovable
    {
        private MovementManager _movementManager;
        private AnimationManager _animationManager;

        public Vector2 Position { get; set; }
        public Vector2 Speed { get; set; }
        public IInputReader InputReader { get; set; }

        public Hero(Texture2D walkTexture, Texture2D idleTexture, IInputReader inputReaderIn)
        {
            InputReader = inputReaderIn;

            var idleAnimation = new IdleAnimation(idleTexture);
            var walkAnimation = new WalkAnimation(walkTexture);

            _animationManager = new AnimationManager(idleAnimation, walkAnimation);
            _movementManager = new MovementManager();

            Position = new Vector2(10, 10);
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

            _movementManager.Move(this);
            _animationManager.Update(direction, gameTime);
        }
    }
}