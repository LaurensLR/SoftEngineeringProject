using CherryCollector.Core;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CherryCollector.Levels
{
    /// <summary>
    ///         Level CLASS - LEVEL DATA AND PARSING     
    ///   PURPOSE:
    ///   Manages level data storage and parsing of string-based level maps into     
    ///   actual game objects. Acts as the bridge between raw data and game world.   
    ///   COORDINATE SYSTEM:
    ///     • X position = column index × tileSize (16 pixels)     
    ///     • Y position = row index × tileSize (16 pixels)   
    ///     • Top-left is (0, 0)     
    ///   DESIGN PATTERNS APPLIED:     
    ///   [FACTORY PATTERN - Client]       
    ///   Level is a CLIENT of LevelObjectFactory. It:     
    ///     • Doesn't know HOW to create Tiles, Spikes, Cherries  
    ///     • Delegates object creation to the factory   
    ///     • Only knows WHAT characters mean (# = block, ^ = spike)    
    ///   [REPOSITORY PATTERN - Consumer]       
    ///   Level consumes data from LevelRepository:  
    ///     • Doesn't store raw level strings itself    
    ///  • Fetches level data from repository on initialization    
    ///     • Separation of data storage from data usage
    ///   SOLID PRINCIPLES APPLIED:        
    ///   [S] Single Responsibility Principle (SRP):    
    ///       Level ONLY parses maps and stores level objects.
    ///       It doesn't:       
    ///      • Create game objects (Factory's job)  
    ///         • Store raw level data (Repository's job)   
    ///         • Update or draw objects (LevelManager's job)     
    ///   [D] Dependency Inversion Principle (DIP):  
    ///     Level depends on LevelObjectFactory abstraction.     
    ///       Factory is injected via constructor (Dependency Injection).  
    ///   [O] Open/Closed Principle (OCP):     
    ///       New object types can be added by:   
    ///    1. Adding new character mapping in Factory
    ///         2. Using new character in level strings   
    ///       Level class doesn't need modification.   
    /// </summary>
    public class Level
    {
        // Use IGameObject to support both physics and drawing
        public List<IGameObject> LevelObjects { get; private set; } = new();
        public int LevelCount => _levels.Count; // Expose the count
        private readonly int _tileSize = 16;
        private readonly List<string[]> _levels = new();
        private readonly LevelObjectFactory _factory;

        public Level(LevelObjectFactory factory)
        {
            _factory = factory;
            InitializeLevels();
            LoadLevel(0);
        }

        private void InitializeLevels()
        {
            // DESIGN PATTERN - Repository or Data Access Object (DAO) usage
            var rawLevels = LevelRepository.GetLevels();
            _levels.AddRange(rawLevels);
        }

        public void LoadLevel(int index)
        {
            if (index < 0 || index >= _levels.Count) return;

            LevelObjects.Clear();
            string[] map = _levels[index];

            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    // The factory now returns an object that implements IGameObject
                    var obj = _factory.CreateObject(map[y][x], new Vector2(x * _tileSize, y * _tileSize), _tileSize);
                    if (obj != null) LevelObjects.Add(obj);
                }
            }
        }
    }
}