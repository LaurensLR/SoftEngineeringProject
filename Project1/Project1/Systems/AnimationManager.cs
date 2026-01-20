using CherryCollector.Graphics;
using Microsoft.Xna.Framework;

namespace CherryCollector.Systems
{
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

        /* 
         * SOLID - Single Responsibility: 
         * This method provides a clean way to force the manager back to a starting state
         * without recreating the whole object.
         */
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