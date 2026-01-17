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
        
        // FIXED: Decouple physics size from animation frame size
        // This prevents the player from "teleporting" or getting stuck when 
        // switching to animations with different dimensions (like Hurt/Death)
        private const int FixedWidth = 26;
        private const int FixedHeight = 28;

        public int Width => FixedWidth;
        public int Height => FixedHeight;

        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        public CollisionType CollisionType => CollisionType.Hero;

        private AnimationManager _animationManager;
        private MovementManager _movementManager;
        private JumpManager _jumpManager;

        private int _lives = 3;
        private bool _isDead = false;
        private float _hurtTimer = 0f;
        private const float HurtCooldown = 1f;

        public Hero(Texture2D walkTexture, Texture2D idleTexture, Texture2D hurtTexture, Texture2D deathTexture, IInputReader inputReader)
        {
            InputReader = inputReader;

            var idle = new IdleAnimation(idleTexture);
            var walk = new WalkAnimation(walkTexture);
            var hurt = new HurtAnimation(hurtTexture);
            var death = new DeathAnimation(deathTexture);

            _animationManager = new AnimationManager(idle, walk, hurt, death);
            _movementManager = new MovementManager();
            _jumpManager = new JumpManager();

            Speed = new Vector2(2f, 0);
        }

        public void ResetResponse(Vector2 startPos)
        {
             Position = startPos;
             _lives = 3;
             _isDead = false;
             _hurtTimer = 0;
             // Just resetting position is enough; animation state updates automatically on input
        }

        public void Update(GameTime gameTime, List<ICollidable> worldObjects)
        {
            if (!_isDead)
            {
                _movementManager.MoveHorizontally(this, worldObjects);
                _movementManager.MoveVertically(this, _jumpManager, worldObjects);

                if (InputReader.ReadInput().Y > 0)
                    _jumpManager.Jump();
            }

            Vector2 input = !_isDead ? new Vector2(InputReader.ReadInput().X, 0) : Vector2.Zero;
            _animationManager.Update(input, gameTime);

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

            if (_hurtTimer > 0)
                _hurtTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (Position.Y > 2000) Die();
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
            var anim = _animationManager.CurrentAnimation;
            if (anim == null || anim.CurrentFrame == null || anim.Texture == null) return;

            var effect = _animationManager.FacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(anim.Texture, Position,
                             anim.CurrentFrame.SourceRectangle,
                             Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
        }

        public void OnCollision(ICollidable other) { }
    }
}
