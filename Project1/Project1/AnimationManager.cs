using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Project1
{
    internal class AnimationManager
    {
        private Animation _idleAnimation;
        private Animation _walkAnimation;
        private Animation _hurtAnimation;
        private Animation _deathAnimation;
        private Animation _currentAnimation;
        private bool _facingLeft;
        public Animation CurrentAnimation => _currentAnimation;
        public bool FacingLeft => _facingLeft;
        public AnimationManager(Animation idleAnimation, Animation walkAnimation, Animation hurtAnimation, Animation deathAnimation)
        {
            _idleAnimation = idleAnimation;
            _walkAnimation = walkAnimation;
            _hurtAnimation = hurtAnimation;
            _deathAnimation = deathAnimation;
            _currentAnimation = _idleAnimation;
            _facingLeft = false;
        }
        public void Update(Vector2 direction, GameTime gameTime)
        {
            bool isMoving = direction != Vector2.Zero;
            // Update facing direction
            if (direction.X < 0)
            {
                _facingLeft = true;
            }
            else if (direction.X > 0)
            {
                _facingLeft = false;
            }
            // Switch animations based on movement
            if (isMoving && _currentAnimation != _walkAnimation)
            {
                _currentAnimation = _walkAnimation;
            }
            else if (!isMoving && _currentAnimation != _idleAnimation)
            {
                _currentAnimation = _idleAnimation;
            }
            _currentAnimation.Update(gameTime);
        }
    }
}