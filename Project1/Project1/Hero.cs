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
        private const float HurtCooldown = 1.0f; // 1 second of invulnerability

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
        }

        public void Update(GameTime gameTime, List<ICollidable> worldObjects)
        {
            if (!_isDead)
            {
                // Only allow player control if NOT currently hurt/bouncing (optional choice)
                // For better feel, we usually allow air control even when hurt, but bounce dominates momentarily.
                _movementManager.MoveHorizontally(this, worldObjects);
                _movementManager.MoveVertically(this, _jumpManager, worldObjects);

                if (InputReader.ReadInput().Y > 0)
                    _jumpManager.Jump();
            }

            Vector2 input = !_isDead ? new Vector2(InputReader.ReadInput().X, 0) : Vector2.Zero;
            _animationManager.Update(input, gameTime);

            // Handle spikes - only if not already under invulnerability period
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
            Bounce();
        }

        private void Bounce()
        {
            // Apply a vertical jump impulse ("pop up")
            _jumpManager.VelocityY = -5f; 

            // Apply horizontal knockback based on direction
            // If facing left, bounce right. If facing right, bounce left.
            // Since we don't have a dedicated "velocity" vector for X in this simple setup
            // (MoveHorizontally calculates it per frame from input), we can just manually nudge Position 
            // slightly to "start" the bounce away from the spike, or rely on the visual "Jump" to clear it.
            
            // For a simple "Super Mario" style damage hop, just resetting vertical velocity is usually enough 
            // to break contact with the spike below you.
            
            // If you want horizontal knockback (pushed away):
            float pushDir = _animationManager.FacingLeft ? 10f : -10f; // Push opposite to face
            Position = new Vector2(Position.X + pushDir, Position.Y - 2); // Slight immediate nudge up/away
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
            // Flicker effect: 
            // If hurtTimer is active, only draw every ~0.1 seconds (simple modulo check)
            if (_hurtTimer > 0)
            {
                // "Flicker" logic: check if the timer (in tenths of a second) is even/odd
                int flicker = (int)(_hurtTimer * 20); // * 20 gives a fast strobe
                if (flicker % 2 == 0) 
                    return; // Skip drawing this frame to create "invisible" blink
            }

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
