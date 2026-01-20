using CherryCollector.Graphics;
using Microsoft.Xna.Framework;

namespace CherryCollector.Systems
{
    /// <summary>
    /// AnimationManager CLASS - ANIMATION STATE MACHINE   
    ///   PURPOSE:    
    ///   Manages which animation is currently playing and handles transitions        
    ///   between animations based on entity state (idle, walking, hurt, dead).      
    ///   FACING DIRECTION:     
    ///   • FacingLeft tracks which way the sprite should face 
    ///   • Set when X direction changes (left = -1, right = +1)  
    ///   • Used by Draw() to flip sprite with SpriteEffects.FlipHorizontally
    ///   DESIGN PATTERNS APPLIED:   
    ///   [STATE MACHINE PATTERN]    
    ///   AnimationManager implements a simple state machine:     
    ///     • States: Idle, Walk, Hurt, Dead (AnimationState enum)  
    ///     • Transitions: Based on input direction and method calls    
    ///     • Each state has an associated Animation object    
    ///   [COMPONENT PATTERN]    
    ///   AnimationManager is a reusable component used by:   
    ///     • Hero (for player animations)     
    ///  • All Enemy subclasses (Snail, Bat)
    ///   This avoids duplicating animation logic in each entity. 
    ///   SOLID PRINCIPLES APPLIED:     
    ///   [S] Single Responsibility Principle (SRP):  
    ///       AnimationManager ONLY handles animation state and transitions.  
    ///       It doesn't:
    ///         • Know about physics or input   
    ///         • Handle drawing (just provides CurrentAnimation)   
    ///         • Calculate frame timing (delegated to Animation class)    
    ///   [O] Open/Closed Principle (OCP):   
    ///       New animation states (jumping, attacking) can be added by:   
    ///    • Adding new AnimationState enum values     
    ///         • Adding new Animation fields and Play methods  
    ///       Existing code doesn't need modification.  
    /// </summary>

    internal enum AnimationState
    {
        Idle,
        Walk,
        Hurt,
        Dead
    }

    public class AnimationManager
    {
        private Animation _idleAnimation;
        private Animation _walkAnimation;
        private Animation _hurtAnimation;
        private Animation _deathAnimation;
        private Animation _currentAnimation;
        private AnimationState _state = AnimationState.Idle;

        private bool _facingLeft;
        public Animation CurrentAnimation => _currentAnimation;
        public bool FacingLeft => _facingLeft;

        public AnimationManager(Animation idleAnimation, Animation walkAnimation, Animation hurtAnimation, Animation deathAnimation)
        {
            _idleAnimation = idleAnimation;
            _walkAnimation = walkAnimation;
            _hurtAnimation = hurtAnimation;
            _deathAnimation = deathAnimation;
            _currentAnimation = idleAnimation;
            _facingLeft = false;
        }


        public void Reset()
        {
            _state = AnimationState.Idle;
            _currentAnimation = _idleAnimation;
            _currentAnimation.Reset();
            _facingLeft = false;
        }

        public void Update(Vector2 direction, GameTime gameTime)
        {
            if (_state == AnimationState.Dead)
            {
                // Always update death animation until it finishes
                _currentAnimation.Update(gameTime);
                return;
            }

            if (_state == AnimationState.Hurt)
            {
                _currentAnimation.Update(gameTime);

                if (_currentAnimation.IsFinished)
                {
                    _state = AnimationState.Idle;
                    _currentAnimation = _idleAnimation;
                    _currentAnimation.Reset();
                }
                return;
            }


            // Facing direction
            if (direction.X < 0)
                _facingLeft = true;
            else if (direction.X > 0)
                _facingLeft = false;

            // Movement animations
            if (direction.X != 0)
            {
                if (_state != AnimationState.Walk)
                {
                    _state = AnimationState.Walk;
                    _currentAnimation = _walkAnimation;
                    _currentAnimation.Reset();
                }
            }
            else
            {
                if (_state != AnimationState.Idle)
                {
                    _state = AnimationState.Idle;
                    _currentAnimation = _idleAnimation;
                    _currentAnimation.Reset();
                }
            }

            _currentAnimation.Update(gameTime);
        }


        public void PlayHurt()
        {
            if (_state == AnimationState.Hurt) return;

            _state = AnimationState.Hurt;
            _currentAnimation = _hurtAnimation;
            _currentAnimation.Reset();
        }



        public void PlayDeath()
        {
            _state = AnimationState.Dead;
            _currentAnimation = _deathAnimation;
            _currentAnimation.Reset();
        }
    }
}