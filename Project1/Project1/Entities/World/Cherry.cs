using CherryCollector.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CherryCollector.Entities.World
{
    /// <summary>
    ///          Cherry CLASS - COLLECTIBLE ITEM   
    ///   PURPOSE:      
    ///   Represents a collectible cherry that the Hero must gather to unlock the    
    ///   level's exit door. All cherries must be collected before completing level. 
    ///   DESIGN PATTERNS APPLIED:  
    ///   [FACTORY PATTERN - Product] 
    ///   Cherry is created by LevelObjectFactory when it encounters 'C' character.  
    ///   Level doesn't instantiate Cherry directly.    
    ///   [OBSERVER PATTERN (implicit)]     
    ///   The Cherry's state change (IsCollected = true) is observed by:   
    ///     • LevelManager: Checks AllCherriesCollected() for door unlock     
    ///     • LevelManager.Update(): Removes collected cherries 
    ///   SOLID PRINCIPLES APPLIED:    
    ///   [S] Single Responsibility Principle (SRP):         
    ///       Cherry ONLY tracks its own collected state and visual.   
    ///       It doesn't update score, play sounds, or check win conditions.
    ///   [O] Open/Closed Principle (OCP):   
    ///       New collectible types (coins, gems) can be added following same
    ///  pattern without modifying Cherry or collection logic.
    ///   [L] Liskov Substitution Principle (LSP):
    ///       Cherry can replace any IGameObject in collections.
    ///       LevelManager iterates List<IGameObject> polymorphically.     
    /// </summary>


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