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
    /// <summary>
    ///         Hero CLASS - PLAYER CHARACTER       
    ///   PURPOSE:   
    ///   The main player-controlled character. Handles input, movement, collisions, 
    ///   animations, damage, and death. The Hero is the central entity of gameplay. 
    ///   COMPONENT DEPENDENCIES:    
    ///     • PhysicsManager: Handles gravity, velocity, collision resolution  
    ///     • AnimationManager: Manages sprite animations (idle, walk, hurt, death)  
    ///     • IInputReader: Abstracts player input (keyboard, gamepad, etc.)      
    ///     • CollisionManager: Detects collisions with world objects     
    /// • LevelManager: Provides access to current level objects  
    ///   DESIGN PATTERNS APPLIED:     
    ///   [STATE PATTERN]  
    ///   Hero uses IHeroState to change behavior at runtime: 
    ///     • NormalState: Full player control, physics active  
    ///     • DeadState: No input, death animation plays    
    ///   SetState() transitions between states, Enter() initializes new state.      
    ///   [OBSERVER PATTERN]  
    /// Hero publishes events that other objects subscribe to:     
    ///  • LivesChanged: UIManager subscribes to update HUD display     
    ///     • Died: Game1 subscribes to trigger GameOverState
    ///   [STRATEGY PATTERN]    
    ///   IInputReader allows swapping input methods without changing Hero code.     
    ///   KeyBoardReader is the current strategy; others can be added.  
    ///   [DELEGATION PATTERN] 
    ///   Hero delegates responsibilities to specialized managers: 
    ///     • Physics → PhysicsManager     
    ///   • Animation → AnimationManager      
    ///     • Input → IInputReader  
    ///   SOLID PRINCIPLES APPLIED:
    ///   [S] Single Responsibility Principle (SRP):    
    ///       Hero coordinates its subsystems but doesn't implement physics or       
    ///       animation logic itself. Each responsibility is delegated.  
    ///   [O] Open/Closed Principle (OCP):  
    ///       New states (PowerUpState, InvincibleState) can be added without
    ///       modifying Hero - just create new IHeroState implementations.      
    ///   [D] Dependency Inversion Principle (DIP):    
    ///       Hero depends on abstractions (IInputReader, IHeroState), not concrete  
    ///       implementations. This enables testing and flexibility.   
    ///   [I] Interface Segregation Principle (ISP): 
    ///       Hero implements IGameObject and IMovable - exactly what's needed.      
    ///       IMovable adds movement properties (Position, Speed, Width, Height).   
    /// </summary>
    public class Hero : IGameObject, IMovable
    {
        private const int FixedWidth = 26;
        private const int FixedHeight = 28;
        public int MaxLives { get; set; } = 3;

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


        public PhysicsManager PhysicsManager { get; private set; }

        public LevelManager LevelManager { get; private set; }
        // SOLID - Dependency Inversion: We depend on the manager, not the implementation details.
        private CollisionManager _collisionManager;

        private IHeroState _currentState;
        private int _lives;

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


        public Hero(Texture2D walk, Texture2D idle, Texture2D hurt, Texture2D death, IInputReader input, LevelManager levelManager, CollisionManager collisionManager, PhysicsManager physicsManager)
        {
            InputReader = input;
            LevelManager = levelManager;
            _collisionManager = collisionManager;
            PhysicsManager = physicsManager;

            AnimationManager = new AnimationManager(
                new IdleAnimation(idle),
                new WalkAnimation(walk),
                new HurtAnimation(hurt),
                new DeathAnimation(death));

            Speed = new Vector2(150f, 0);

            // Set initial state
            SetState(new NormalState());
        }


        public void SetState(IHeroState newState)
        {
            _currentState = newState;
            _currentState.Enter(this);
        }

        public void Update(GameTime gameTime)
        {
            _currentState.Update(this, gameTime);


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

        }


        public void Reset(Vector2 spawnPosition)
        {
            Position = spawnPosition;
            Lives = MaxLives;
            _hurtTimer = 0;
            Speed = new Vector2(150f, 0);

            // Clean reset of physics
            PhysicsManager.Reset();
            AnimationManager.Reset();

            SetState(new NormalState());
        }

        // Helper for level transitions
        public void ResetHero(Vector2 newPos)
        {
            Lives = MaxLives;
            Position = newPos;
            PhysicsManager.Reset();
        }

        private void TakeDamage(IGameObject spike)
        {
            Lives--;
            _hurtTimer = HurtCooldown;

            if (Lives <= 0) SetState(new DeadState());
            else
            {
                AnimationManager.PlayHurt();
                Bounce(spike);
            }
        }

        private void Bounce(IGameObject hazard)
        {
            // Delegate bounce physics to the manager
            PhysicsManager.ApplyBounce();

            // Minimal horizontal nudge
            float bounceAmount = 2f;
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
