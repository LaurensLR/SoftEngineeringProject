using CherryCollector.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CherryCollector.Entities.World
{
    /// <summary>
    ///      Spike CLASS - HAZARD OBJECT  
    ///   PURPOSE:     
    /// Represents a static hazard that damages the Hero on contact.     
    ///   Spikes are placed on platforms to create challenging obstacles.
    ///   DESIGN PATTERNS APPLIED: 
    ///   [FACTORY PATTERN - Product] 
    ///   Spike is a product of LevelObjectFactory. The Level class doesn't know     
    /// how to create Spikes - it delegates to the factory.   
    ///   [TELL, DON'T ASK]  
    ///   Spike doesn't ask Hero "are you touching me?" - instead, CollisionManager  
    ///   tells both objects they've collided, and Hero handles the damage.
    ///   SOLID PRINCIPLES APPLIED:  
    ///   [S] Single Responsibility Principle (SRP):   
    ///       Spike ONLY represents a hazard's position and visual.  
    ///       It doesn't calculate damage - that's Hero's responsibility.
    ///       It doesn't check collisions - that's CollisionManager's job.     
    ///   [O] Open/Closed Principle (OCP):    
    ///   New hazard types (fire, acid, etc.) can be added by creating new   
    ///       classes with CollisionType.Spike without modifying Spike class.    
    /// </summary>
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

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destRect = new Rectangle((int)Position.X, (int)Position.Y, 16, 13);
            spriteBatch.Draw(_texture, destRect, _sourceRect, Color.White);
        }

        public void OnCollision(IGameObject other)
        {

        }
    }
}