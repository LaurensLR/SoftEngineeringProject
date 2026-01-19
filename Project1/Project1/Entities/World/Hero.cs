using CherryCollector.Core;
using CherryCollector.Levels;
using CherryCollector.States.HeroStates;
using CherryCollector.Systems;
using CherryCollector.Systems.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CherryCollector.Graphics;
using System;

namespace CherryCollector.Entities.World
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

        public AnimationManager AnimationManager { get; private set; }
        public MovementManager MovementManager { get; private set; }
        public JumpManager JumpManager { get; private set; }
        public LevelManager LevelManager { get; private set; }
        // SOLID - Dependency Inversion: We depend on the manager, not the implementation details.
        private CollisionManager _collisionManager;

        private IHeroState _currentState;
        private int _lives = 3;

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

        public Hero(Texture2D walk, Texture2D idle, Texture2D hurt, Texture2D death, IInputReader input, LevelManager levelManager, CollisionManager collisionManager)
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
            _collisionManager = collisionManager;
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
            _currentState.Update(this, gameTime);

            /* 
             * SOLID - SRP Asking the CollisionManager for collisions:
             */
            _collisionManager.CheckCollisions(this, LevelManager.CurrentLevelObjects);

            if (_hurtTimer > 0)
                _hurtTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Position.Y > 2000 && !(_currentState is DeadState))
                SetState(new DeadState());
        }

        public void OnCollision(IGameObject other)
        {
            if (_currentState is DeadState || _hurtTimer > 0) return;

            if (other.CollisionType == CollisionType.Spike || other.CollisionType == CollisionType.Enemy)
            {
                TakeDamage(other);
            }
            // Cherry & Door logic handled elsewhere or by LevelManager queries
        }

        /* 
         * SOLID - Single Responsibility: Provides a way to reset character state 
         * for the restart functionality.
         */
        public void Reset(Vector2 spawnPosition)
        {
            Position = spawnPosition;
            Lives = 3;
            _hurtTimer = 0;
            Speed = new Vector2(150f, 0);

            /* 
             * DESIGN PATTERN - Cleanup: 
             * Explicitly resetting the animation manager ensures visual state 
             * matches the logic state after a restart.
             */
            AnimationManager.Reset();

            SetState(new NormalState());
        }

        // Helper for level transitions
        public void ResetPosition(Vector2 newPos)
        {
            Position = newPos;
            JumpManager.VelocityY = 0;
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

        private void Bounce(IGameObject hazard)
        {
            /* 
             * SOLID - SRP Hero does not need to know HOW to bounce: 
             * We tell the JumpManager to apply the vertical force. 
             * Hero only handles the horizontal nudge logic.
             */
            JumpManager.ApplyBounce();

            float bounceAmount = 15f;
            float pushDir = AnimationManager.FacingLeft ? bounceAmount : -bounceAmount;
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
