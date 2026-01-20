using CherryCollector.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CherryCollector.Entities.Enemies;
using CherryCollector.Entities.World;

namespace CherryCollector.Levels
{
    /// <summary>
    ///   LevelObjectFactory CLASS - GAME OBJECT CREATION  
    ///   PURPOSE:    
    ///   Centralizes the creation of all game objects from level data.     
    ///   Translates character codes into fully configured game entities.
    ///      CREATION PROCESS:   
    ///      1. Level calls CreateObject(char, position, tileSize) 
    ///      2. Factory uses switch expression to match character
    ///      3. Factory creates appropriate object with correct texture   
    ///      4. Returns IGameObject (or null for empty space)   
    ///   TEXTURE MANAGEMENT:
    ///   Factory stores all textures needed for object creation.  
    ///   Textures are injected via constructor (Dependency Injection).   
    ///   This keeps texture loading in Game1, creation logic here.      
    ///   DESIGN PATTERNS APPLIED: 
    ///   [FACTORY PATTERN]     
    ///   Benefits:      
    ///     • Level doesn't need to know how to construct each object type    
    ///     • Object creation logic is centralized in one place
    ///     • Easy to add new object types (add texture, add case)    
    ///     • Consistent object configuration (textures, positions)       
    ///   SOLID PRINCIPLES APPLIED: 
    ///   [S] Single Responsibility Principle (SRP):   
    ///       Factory ONLY creates game objects from characters.   
    ///       It doesn't load textures, parse levels, or update objects.       
    ///   [O] Open/Closed Principle (OCP):   
    ///    OPEN: New object types can be added by:     
    ///         1. Add texture parameter to constructor  
    ///         2. Add new case to switch expression     
    ///       CLOSED: Existing Level parsing code doesn't change.     
    ///   [D] Dependency Inversion Principle (DIP):  
    ///       Factory returns IGameObject interface, not concrete types.  
    ///    Level doesn't know if it got a Tile or Spike - just IGameObject.       
    /// </summary>
    public class LevelObjectFactory
    {
        private readonly Texture2D _tileTexture, _spikeTexture, _cherryTexture, _doorTexture, _snailTexture, _batTexture;

        public LevelObjectFactory(Texture2D tile, Texture2D spike, Texture2D cherry, Texture2D door, Texture2D snail, Texture2D bat)
        {
            _tileTexture = tile;
            _spikeTexture = spike;
            _cherryTexture = cherry;
            _doorTexture = door;
            _snailTexture = snail;
            _batTexture = bat;
        }

        public IGameObject CreateObject(char type, Vector2 position, int tileSize)
        {
            return type switch
            {
                '#' => new Tile(_tileTexture, new Rectangle((int)position.X, (int)position.Y, tileSize, tileSize)),
                '^' => new Spike(_spikeTexture, new Vector2(position.X, position.Y + 3)),
                'C' => new Cherry(_cherryTexture, position),
                'D' => new Door(_doorTexture, position),
                'S' => new Snail(_snailTexture, position),
                'B' => new Bat(_batTexture, position),

                _ => null
            };
        }
    }
}