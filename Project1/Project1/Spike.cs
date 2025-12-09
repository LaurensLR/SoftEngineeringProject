using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project1
{
    public class Spike : ICollidable
    {
        public Vector2 Position { get; private set; }
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, 32, 26);
        public CollisionType CollisionType => CollisionType.Spike;

        private Texture2D _texture;
        private Rectangle _sourceRect;

        public Spike(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            Position = position;

            // Only use the actual spike portion of the texture (top-left 16x13)
            _sourceRect = new Rectangle(0, 0, 16, 13);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destRect = new Rectangle((int)Position.X, (int)Position.Y, 32, 26);

            spriteBatch.Draw(_texture, destRect, _sourceRect, Color.White);
        }


        public void OnCollision(ICollidable other)
        {
            // Spike logic handled by Hero
        }
    }
}
