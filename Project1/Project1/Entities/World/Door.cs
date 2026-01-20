using CherryCollector.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CherryCollector.Entities.World
{
    /// <summary>
    ///          Door CLASS - LEVEL EXIT  
    ///   PURPOSE:       
    ///   Represents the exit door that completes a level when the Hero enters it    
    ///   after collecting all cherries. Acts as both a visual landmark and goal.   
    ///   DESIGN PATTERNS APPLIED:
    ///   [FACTORY PATTERN - Product] 
    ///   Door is created by LevelObjectFactory, keeping Level class clean.    
    ///   [STATE TRACKING]   
    ///   IsPlayerInside is a simple state that's reset each frame and set on   
    ///   collision. This "pulse" pattern prevents stale state.  
    ///   SOLID PRINCIPLES APPLIED:
    ///   [S] Single Responsibility Principle (SRP):  
    ///       Door ONLY tracks whether the Hero is inside and renders itself.         
    ///       Level completion logic is in LevelManager, not Door.  
    ///   [O] Open/Closed Principle (OCP):    
    ///     Door behavior can be extended (locked doors, keys) by adding new   
    ///   door types without modifying existing Door class.   
    /// </summary>
    public class Door : IGameObject
    {
        private readonly Texture2D _texture;
        public Vector2 Position { get; private set; }


        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, 32, 48);
        public CollisionType CollisionType => CollisionType.Door;

        public bool IsPlayerInside { get; private set; }

        public Door(Texture2D texture, Vector2 position)
        {
            _texture = texture;

            Position = new Vector2(position.X, position.Y - 24);
        }

        public void Update(GameTime gameTime)
        {
            IsPlayerInside = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

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