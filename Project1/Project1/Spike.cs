using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project1
{
    public class Spike : IGameObject
    {
        public Vector2 Position { get; private set; }
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, 16, 13);
        public CollisionType CollisionType => CollisionType.Spike;

        private Texture2D _texture;
        private Rectangle _sourceRect;

        public Spike(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            Position = position;
            _sourceRect = new Rectangle(0, 0, 16, 13);
        }

        public void Update(GameTime gameTime)
        {
            // Spikes are currently static.
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destRect = new Rectangle((int)Position.X, (int)Position.Y, 16, 13);
            spriteBatch.Draw(_texture, destRect, _sourceRect, Color.White);
        }

        public void OnCollision(IGameObject other)
        {
            // Logic handled by the Hero when it hits this spike.
        }
    }
}
