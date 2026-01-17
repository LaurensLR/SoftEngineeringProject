using Microsoft.Xna.Framework;

namespace Project1
{
    public class JumpManager
    {
        private float _gravity = 0.5f;
        private float _jumpStrength = -8.5f; // Tuned for 16px tiles: ~70px height (approx 4.5 blocks)
        private float _velocityY = 0f;
        private bool _isGrounded = false;

        public float VelocityY
        {
            get => _velocityY;
            set => _velocityY = value;
        }

        public bool IsGrounded => _isGrounded;

        public JumpManager()
        {
        }

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
                _velocityY *= 0.5f; 
        }

        public void Land()
        {
            _velocityY = 0f;
            _isGrounded = true;
        }

        public float Update(IMovable movable)
        {
            _velocityY += _gravity;
            _isGrounded = false; 
            return _velocityY;
        }
    }
}
