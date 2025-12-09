using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project1
{
    public class Block : ICollidable
    {
        public Rectangle Bounds { get; private set; }
        public CollisionType CollisionType => CollisionType.Block;

        private Texture2D _texture;

        public Block(Texture2D texture, Rectangle bounds)
        {
            _texture = texture;
            Bounds = bounds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Bounds, Color.Brown);
        }

        public void OnCollision(ICollidable other)
        {
            // Blocks don't react
        }
    }
}
