using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project1.Input;
using System;
using System.Collections.Generic;

namespace Project1
{
    /* 
     * SOLID - Single Responsibility Principle (SRP):
     * Hero is responsible for Hero logic (health, input, animation state). 
     * It delegates Physics to Movement/Jump Managers and behavior to States.
     * 
     * SOLID - Dependency Inversion Principle (DIP):
     * Hero depends on LevelManager (an abstraction/manager) to see the world.
     */
    public class Hero : IGameObject, IMovable
    {
        private const int FixedWidth = 26;
        private const int FixedHeight = 28;

        public Vector2 Position { get; set; }
        public Vector2 Speed { get; set; }
        public IInputReader InputReader { get; set; }
        public int Width => FixedWidth;
        public int Height => FixedHeight;
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        public CollisionType CollisionType => CollisionType.Hero;

        // DESIGN PATTERN - Observer Pattern: Events that other classes (Observers) can subscribe to
        public event EventHandler<int> LivesChanged;
        public event EventHandler Died;

        // Managers are internal/protected so States can access them
        internal AnimationManager AnimationManager { get; private set; }
        internal MovementManager MovementManager { get; private set; }
        internal JumpManager JumpManager { get; private set; }
        internal LevelManager LevelManager { get; private set; }

        private IHeroState _currentState;
        private int _lives = 1000000;

        // Encapsulated property that notifies observers via the Observer Pattern
        public int Lives
        {
            get => _lives;
            private set
            {
                _lives = value;
                // Notify Observers that something changed
                LivesChanged?.Invoke(this, _lives);

                if (_lives <= 0)
                {
                    Died?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private float _hurtTimer;
        private const float HurtCooldown = 1.0f;

        public Hero(Texture2D walk, Texture2D idle, Texture2D hurt, Texture2D death, IInputReader input, LevelManager levelManager)
        {
            InputReader = input;
            LevelManager = levelManager;
            AnimationManager = new AnimationManager(
                new IdleAnimation(idle), 
                new WalkAnimation(walk), 
                new HurtAnimation(hurt), 
                new DeathAnimation(death));
            MovementManager = new MovementManager();
            JumpManager = new JumpManager();
            Speed = new Vector2(150f, 0);

            // Set initial state
            SetState(new NormalState());
        }

        /*
         * DESIGN PATTERN - State Pattern Switch:
         * This method handles changing the behavior of the hero.
         */
        public void SetState(IHeroState newState)
        {
            _currentState = newState;
            _currentState.Enter(this);
        }

        public void Update(GameTime gameTime)
        {
            // Delegate ALL update logic to the current state
            _currentState.Update(this, gameTime);

            CheckCollisions(LevelManager.CurrentLevelObjects);

            if (_hurtTimer > 0)
                _hurtTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Position.Y > 2000 && !(_currentState is DeadState)) 
                SetState(new DeadState());
        }

        private void CheckCollisions(List<IGameObject> worldObjects)
        {
            foreach (var obj in worldObjects)
            {
                // Polymorphism: We treat every item in the list as IGameObject
                if (obj != this && Bounds.Intersects(obj.Bounds))
                {
                    obj.OnCollision(this); 
                    this.OnCollision(obj);
                }
            }
        }

        public void OnCollision(IGameObject other)
        {
            if (_currentState is DeadState || _hurtTimer > 0) return;

            if (other.CollisionType == CollisionType.Spike)
            {
                // DESIGN PATTERN - Strategy/Observer:
                // We pass the spike's reference to TakeDamage so we can calculate bounce direction.
                TakeDamage(other);
            }
        }

        private void TakeDamage(IGameObject spike)
        {
            Lives--;
            _hurtTimer = HurtCooldown;

            if (Lives <= 0) SetState(new DeadState());
            else
            {
                AnimationManager.PlayHurt();
                // We now pass the spike to the Bounce method
                Bounce(spike);
            }
        }

        private void Bounce(IGameObject spike)
        {
            /* 
             * SOLID - Frame Independence:
             * Using pixels-per-second values for a clear vertical "pop".
             * This upward movement is handled by JumpManager and is smooth (over time).
             */
            JumpManager.VelocityY = -350f;

            /* 
             * CALCULATING TRAJECTORY:
             * We compare the horizontal center of the Hero with the horizontal center 
             * of the Spike to determine which way to "bounce away".
             */
            float heroCenter = Bounds.Center.X;
            float spikeCenter = spike.Bounds.Center.X;

            /*
             * BUG FIX - Smoother Knockback:
             * Previously, pushAmount was 30f, which caused an instant teleport.
             * We reduce this to a tiny nudge (2f) to break the collision overlap,
             * and rely on the upward JumpManager velocity to create the "arch" away.
             */
            float nudgeAmount = 2f; 
            float pushDir = (heroCenter < spikeCenter) ? -nudgeAmount : nudgeAmount;

            // Apply minor immediate nudge + lift slightly out of the spike
            Position = new Vector2(Position.X + pushDir, Position.Y - 2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_hurtTimer > 0 && (int)(_hurtTimer * 20) % 2 == 0) return;
            var anim = AnimationManager.CurrentAnimation;
            if (anim?.CurrentFrame == null) return;

            var effect = AnimationManager.FacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(anim.Texture, Position, anim.CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
        }
    }
}
