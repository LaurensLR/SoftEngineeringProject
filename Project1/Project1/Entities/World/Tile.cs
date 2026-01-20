using CherryCollector.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CherryCollector.Entities.World
{
    /// <summary>
    ///       Tile CLASS - SOLID PLATFORM BLOCK 
    ///   PURPOSE:         
    ///   Represents a solid, static tile that forms the platforms and walls of      
    ///   the game world. Heroes can stand on tiles, and they block movement.  
    ///   DESIGN PATTERNS APPLIED:           
    ///   [FACTORY PATTERN - Product]            
    ///   Tile is created by LevelObjectFactory, not instantiated directly.
    ///   The factory reads '#' from the level map and creates Tile objects.   
    ///   [NULL OBJECT PATTERN (partial)]    
    ///   The Update() and OnCollision() methods exist to satisfy IGameObject,
    ///   but they do nothing. This is intentional - Tiles are passive objects.
    ///   SOLID PRINCIPLES APPLIED: 
    ///   [S] Single Responsibility Principle (SRP):     
    ///       Tile ONLY represents a solid block in the world.
    ///       It doesn't handle its own collision resolution - that's PhysicsManager.
    ///   [I] Interface Segregation Principle (ISP): 
    ///   Tile implements IGameObject which provides exactly what's needed:
    ///       Bounds, CollisionType, Update(), Draw(), OnCollision().
    ///       No unused interface methods. 
    ///   [L] Liskov Substitution Principle (LSP):  
    ///       Tile can be used anywhere an IGameObject is expected.     
    ///       LevelManager stores List<IGameObject> containing Tiles, Spikes, etc.   
    /// </summary>
    public class Tile : IGameObject
    {
        public Rectangle Bounds { get; private set; }
        public CollisionType CollisionType => CollisionType.Block;

        private Texture2D _texture;

        public Tile(Texture2D texture, Rectangle bounds)
        {
            _texture = texture;
            Bounds = bounds;
        }


        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Bounds, Color.Brown);
        }

        public void OnCollision(IGameObject other)
        {

        }
    }
}
