using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project1
{
    public class Block : IGameObject
    {
        public Rectangle Bounds { get; private set; }
        public CollisionType CollisionType => CollisionType.Block;

        private Texture2D _texture;

        public Block(Texture2D texture, Rectangle bounds)
        {
            _texture = texture;
            Bounds = bounds;
        }

        /* 
         * SOLID - Interface Implementation:
         * Blocks are static, so the Update method is empty.
         * However, we MUST have it here to fulfill the IGameObject interface.
         */
        public void Update(GameTime gameTime)
        {
            // Static blocks don't need to change over time.
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Bounds, Color.Brown);
        }

        public void OnCollision(IGameObject other)
        {
            // Blocks don't react to collisions, but could in the future (e.g. breakable)
        }
    }
}
