using Microsoft.Xna.Framework;

namespace Project1
{
    internal class JumpManager
    {
        private float _gravity = 0.3f;
        private float _jumpStrength = -5f;
        private float _velocityY = 0f;
        private float _groundLevel;
        private bool _isGrounded = true;

        public bool IsGrounded => _isGrounded;

        public JumpManager(float groundLevel)
        {
            _groundLevel = groundLevel;
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
            if (_velocityY > 0)
                _velocityY = 0f;
        }

        public void Land()
        {
            // called when landing on a platform
            _velocityY = 0f;
            _isGrounded = true;
        }

        public void Update(IMovable movable)
        {
            // Apply gravity
            _velocityY += _gravity;

            // Update position
            movable.Position = new Vector2(movable.Position.X, movable.Position.Y + _velocityY);

            // Check if landed on ground level
            if (movable.Position.Y >= _groundLevel)
            {
                movable.Position = new Vector2(movable.Position.X, _groundLevel);
                _velocityY = 0f;
                _isGrounded = true;
            }
            else
            {
                _isGrounded = false;
            }
        }
    }
}