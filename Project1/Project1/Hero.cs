using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project1.Input;
using System.Collections.Generic;

namespace Project1
{
    public class Hero : ICollidable, IMovable
    {
        public Vector2 Position { get; set; }
        public Vector2 Speed { get; set; }
        public IInputReader InputReader { get; set; }
        public int Width => _animationManager.CurrentAnimation?.CurrentFrame?.SourceRectangle.Width ?? 1;
        public int Height => _animationManager.CurrentAnimation?.CurrentFrame?.SourceRectangle.Height ?? 1;

        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        public CollisionType CollisionType => CollisionType.Hero;

        private AnimationManager _animationManager;
        private MovementManager _movementManager;
        private JumpManager _jumpManager;

        private int _lives = 3;
        private bool _isDead = false;
        private float _hurtTimer = 0f;
        private const float HurtCooldown = 1f;

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
            Speed = new Vector2(2f, 0); // horizontal speed
        }

        public void Update(GameTime gameTime, List<ICollidable> worldObjects)
        {
            if (!_isDead)
            {
                // Horizontal & vertical movement handled by MovementManager
                _movementManager.MoveHorizontally(this, worldObjects);
                _movementManager.MoveVertically(this, _jumpManager, worldObjects);

                // Jump input
                if (InputReader.ReadInput().Y > 0)
                    _jumpManager.Jump();
            }

            // Update animation (always, so death animation can play)
            Vector2 input = !_isDead ? new Vector2(InputReader.ReadInput().X, 0) : Vector2.Zero;
            _animationManager.Update(input, gameTime);

            // Handle spikes
            if (!_isDead && _hurtTimer <= 0)
            {
                foreach (var obj in worldObjects)
                {
                    if (obj.CollisionType == CollisionType.Spike && Bounds.Intersects(obj.Bounds))
                    {
                        TakeDamage();
                        break;
                    }
                }
            }

            // Hurt cooldown
            if (_hurtTimer > 0)
                _hurtTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void TakeDamage()
        {
            _lives--;
            _hurtTimer = HurtCooldown;

            if (_lives <= 0)
            {
                Die();
                return;
            }

            _animationManager.PlayHurt();
        }

        private void Die()
        {
            _isDead = true;
            Speed = Vector2.Zero;
            _jumpManager.CancelJump();
            _animationManager.PlayDeath();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var effect = _animationManager.FacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(_animationManager.CurrentAnimation.Texture, Position,
                             _animationManager.CurrentAnimation.CurrentFrame.SourceRectangle,
                             Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
        }

        public void OnCollision(ICollidable other) { /* spikes handled in Update */ }
    }
}
