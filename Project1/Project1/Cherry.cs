using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project1
{
    /* 
     * SOLID - Single Responsibility Principle (SRP):
     * This class only handles the logic for the Cherry object.
     */
    public class Cherry : IGameObject
    {
        private readonly Texture2D _texture;
        public Vector2 Position { get; private set; }
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, 16, 16);
        public CollisionType CollisionType => CollisionType.Cherry;
        
        public bool IsCollected { get; private set; }

        public Cherry(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            Position = position;
            IsCollected = false;
        }

        public void Update(GameTime gameTime)
        {
            // Static collectible - no update logic needed yet
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsCollected)
            {
                spriteBatch.Draw(_texture, Bounds, Color.White);
            }
        }

        public void OnCollision(IGameObject other)
        {
            if (other.CollisionType == CollisionType.Hero)
            {
                IsCollected = true;
            }
        }
    }
}