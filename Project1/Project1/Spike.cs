using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project1
{
    public class Spike : ICollidable
    {
        public Vector2 Position { get; private set; }
        
        // Fix bounds to match actual texture size (16x13), not 32x26
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, 16, 13);
        public CollisionType CollisionType => CollisionType.Spike;

        private Texture2D _texture;
        private Rectangle _sourceRect;

        public Spike(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            Position = position;

            // Texture source is 16x13
            _sourceRect = new Rectangle(0, 0, 16, 13);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Destination matches source size (no scaling up)
            Rectangle destRect = new Rectangle((int)Position.X, (int)Position.Y, 16, 13);

            spriteBatch.Draw(_texture, destRect, _sourceRect, Color.White);
        }

        public void OnCollision(ICollidable other)
        {
            // Spike logic handled by Hero
        }
    }
}
