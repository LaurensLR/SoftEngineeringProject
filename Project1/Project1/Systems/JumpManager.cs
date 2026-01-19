using Microsoft.Xna.Framework;

namespace CherryCollector.Systems
{
    /* 
    * SOLID - Single Responsibility: 
    * Manages vertical velocity only.
    */
    public class JumpManager
    {
        // Now using pixels per second squared (gravity) and pixels per second (jump)
        private float _gravity = 1500f;
        private float _jumpStrength = -450f;
        private float _bounceStrength = -350f;
        private float _velocityY = 0f;
        private bool _isGrounded = false;

        public float VelocityY
        {
            get => _velocityY;
            set => _velocityY = value;
        }

        public bool IsGrounded => _isGrounded;

        public void Jump()
        {
            if (_isGrounded)
            {
                _velocityY = _jumpStrength;
                _isGrounded = false;
            }
        }

        public void CancelJump()
        {
            if (_velocityY < 0)
                _velocityY = 0f;
        }

        /* 
         * SOLID - Feature Envy Fix:
         * Hero was setting VelocityY directly. We move that logic here.
         */
        public void ApplyBounce()
        {
            _velocityY = _bounceStrength;
            _isGrounded = false;
        }

        public void Land()
        {
            _velocityY = 0f;
            _isGrounded = true;
        }

        /* 
        * REFACTORING - Frame Independence:
        * We multiply our values by deltaTime (seconds passed since last frame).
        * This ensures gravity feels the same regardless of frame rate.
        */
        public float CalculateDeltaY(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update velocity: Acceleration * Time
            _velocityY += _gravity * dt;

            // Reset grounded state (MovementManager will set it to true if we hit a block)
            _isGrounded = false;

            // Return movement distance: Velocity * Time
            return _velocityY * dt;
        }
    }
}
