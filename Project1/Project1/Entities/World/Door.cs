using CherryCollector.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CherryCollector.Entities.World
{
    /* 
     * SOLID - Single Responsibility: Manages the Door's visual and logic state.
     */
    public class Door : IGameObject
    {
        private readonly Texture2D _texture;
        public Vector2 Position { get; private set; }

        /*
         * REFACTORING - Scalability:
         * Made the bounds 2x bigger (32x48) without changing the raw PNG.
         * The physics collision box now matches the new visual scale.
         */
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, 32, 48);
        public CollisionType CollisionType => CollisionType.Door;

        public bool IsPlayerInside { get; private set; }

        public Door(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            // Adjust position so the door sits on the same "floor" level as blocks
            // since it is now 48px high instead of 24px.
            Position = new Vector2(position.X, position.Y - 24);
        }

        public void Update(GameTime gameTime)
        {
            IsPlayerInside = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            /* 
             * DESIGN PATTERN - Visual Scaling:
             * We use the destination rectangle feature of spriteBatch.Draw 
             * to stretch the PNG to our new 32x48 dimensions.
             */
            spriteBatch.Draw(_texture, Bounds, Color.White);
        }

        public void OnCollision(IGameObject other)
        {
            if (other.CollisionType == CollisionType.Hero)
            {
                IsPlayerInside = true;
            }
        }
    }
}